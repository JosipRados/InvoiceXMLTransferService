﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.RepositoryServices;
using System.Xml;
using Renci.SshNet;
using System.Xml.Serialization;
using DataAccess.Models;
using System.IO.Enumeration;
using static System.Net.WebRequestMethods;
using System.IO;

namespace InvoiceXMLTransferService.Services.Implementation
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepositoryService _invoiceRepositoryService;
        private readonly IConfiguration _configuration;
        private string? SFTPServer;
        private string? SFTPUsername;
        private string? SFTPPassword;

        public InvoiceService(IInvoiceRepositoryService invoiceRepositoryService, IConfiguration configuration)
        {
            _invoiceRepositoryService = invoiceRepositoryService;
            _configuration = configuration;
            SFTPServer = _configuration["SFTPConnection:MainServer:ServerIP"];
            SFTPUsername = _configuration["SFTPConnection:MainServer:Username"];
            SFTPPassword = _configuration["SFTPConnection:MainServer:Password"];
        }

        public void InvoiceTransfer()
        {
            try
            {
                List<InvoiceXMLFilesModel>? xmlFiles = FetchDocument();
                if(xmlFiles == null)
                {
                    //log
                    return;
                }
                foreach(var file in xmlFiles)
                {
                    var deserializedInvoices = DeserializeInvoice(file.xml);
                    if (deserializedInvoices == null)
                        throw new Exception("Deserialization of invoices failed!");
                    var invoiceTransferStatus = _invoiceRepositoryService.ImportInvoices(deserializedInvoices);
                    if (invoiceTransferStatus == null)
                        throw new Exception("Import of invoices failed!");
                    SerializeInvoiceTransferStatus(invoiceTransferStatus, file.fileName);
                    MoveInvoiceToArchive(file.fileName);
                }
            }
            catch(Exception ex)
            {
                //log
            }
        }

        internal List<InvoiceXMLFilesModel>? FetchDocument()
        {
            List<InvoiceXMLFilesModel> xmlFiles = new List<InvoiceXMLFilesModel>();
            try
            {
                if (SFTPServer == null || SFTPUsername == null || SFTPPassword == null)
                    throw new Exception("Not valid connection data.");

                using (SftpClient client = new SftpClient(new PasswordConnectionInfo(SFTPServer, SFTPUsername, SFTPPassword)))
                {
                    client.Connect();

                    var fileNames = client.ListDirectory(@"\Invoice_Transfer\Invoice")
                                      .Where(x=>x.Name != "." && x.Name != "..")
                                      .Select(x => x.Name);
                    if (fileNames.Count() < 0 || fileNames == null)
                        return null;

                    foreach(var fileName in fileNames)
                    {
                        InvoiceXMLFilesModel file = new InvoiceXMLFilesModel();
                        using (MemoryStream ms = new MemoryStream())
                        {
                            client.DownloadFile(@"\Invoice_Transfer\Invoice\" + fileName, ms);
                            ms.Position = 0;
                            using (XmlReader reader = XmlReader.Create(ms))
                            {
                                file.xml.Load(reader);
                            }
                            file.fileName = fileName;
                            xmlFiles.Add(file);
                        }
                    }

                    //MemoryStream ms = new MemoryStream();
                    //client.DownloadFile(@"\Invoice_Transfer\Invoice\05052024_InvoiceList001.xml", ms);
                    //ms.Position = 0;

                    //using (XmlReader reader = XmlReader.Create(ms))
                    //{
                    //    xmlFiles.Load(reader);
                    //}
                   
                    client.Disconnect();
                }

                return xmlFiles;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        internal InvoiceListModel? DeserializeInvoice(XmlDocument xml)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(InvoiceListModel));
                using (StringReader reader = new StringReader(xml.OuterXml))
                {
                    return (InvoiceListModel?)serializer.Deserialize(reader);
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        internal void SerializeInvoiceTransferStatus(List<InvoiceTransferStatusModel> transferStatus, string fileName)
        {
            InvoiceStatusResponseModel statusResponse = new InvoiceStatusResponseModel()
            {
                InvoiceName = fileName,
                TransferStatus = transferStatus,
                TimeStamp = DateTime.Now.ToString("yyyy-MM-ddThh:mm:ss")
            };
            try
            {
                using (SftpClient client = new SftpClient(new PasswordConnectionInfo(SFTPServer, SFTPUsername, SFTPPassword)))
                {
                    client.Connect();
                    MemoryStream ms = new MemoryStream();
                    XmlSerializer serializer = new XmlSerializer(typeof(InvoiceStatusResponseModel));
                    serializer.Serialize(ms, statusResponse);
                    ms.Position = 0;
                    client.UploadFile(ms, @"\Invoice_Transfer\InvoiceProcessingResponse\" + fileName + "_Response.xml");
                    
                    client.Disconnect();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        internal void MoveInvoiceToArchive(string fileName)
        {
            using (SftpClient client = new SftpClient(new PasswordConnectionInfo(SFTPServer, SFTPUsername, SFTPPassword)))
            {
                client.Connect();
                var file = client.Get(@"\Invoice_Transfer\Invoice\" + fileName);
                file.MoveTo(@"\Invoice_Transfer\InvoiceArchive\" + fileName);
                client.Disconnect();
            }
        }
    }
}
