using BMotionReporting.Entity;
using BMotionReporting.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace BMotionReporting.Logic
{
    public class DocumentLogic
    {
        private static string pathUpload = ConfigurationManager.AppSettings["PathUploads"];

        BMotionDBEntities db = new BMotionDBEntities();
        List<Document> docList;
        static DocumentLogic doc = null;

        public static DocumentLogic getInstance()
        {
            if (doc == null)
            {
                doc = new DocumentLogic();
                return doc;
            }
            else
            {
                return doc;
            }
        }

        private DocumentLogic()
        {
            docList = db.Documents.ToList();
        }

        public List<Document> getAllDocument()
        {
            return docList;
        }

        public List<DocumentModels> getAllDocumentByUser(string id)
        {
            DocumentModels doc = new DocumentModels();
            List<DocumentModels> dcList = new List<DocumentModels>();
            var list = db.sp_DocumentListByUser(id).ToList();
            foreach(var item in list)
            {
                var docPath = GetDocumentPathByDocNo(item.DocumentNo);
                doc = new DocumentModels();
                doc.No = item.No;
                doc.NIP = id;
                doc.CreatedDate = item.CreatedDate.Value.ToString("MM/dd/yyyy");
                doc.DocumentNo = item.DocumentNo;
                doc.ExpDate = item.ExpDate.Value.ToString("MM/dd/yyyy");
                doc.Quota = item.Quota;
                doc.IsVerify = item.Status;
                doc.DocumentFile = item.DocumentName;
                doc.PathDocument = docPath;
                dcList.Add(doc);
            }
            return dcList;
        }

        private string GetDocumentPathByDocNo(string id)
        {
            return Path.Combine(pathUpload, db.Documents.Where(dc => dc.DocumentNo.Equals(id)).FirstOrDefault().CreatedDate.Value.ToString("ddMMyyyy"), "Document") + "\\" + db.Documents.Where(dc => dc.DocumentNo.Equals(id)).FirstOrDefault().DocumentFile;
        }

        public bool CheckExistingDocument(DocumentModels model)
        {
            if (db.Documents.Where(doc => doc.DocumentNo.Equals(model.DocumentNo)).Count() == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Add(DocumentModels model)
        {
            try
            {
                string dateTimeDayNow = DateTime.Now.ToString("ddMMyyyyHHmmss");
                string dateDayNow = DateTime.Now.ToString("ddMMyyyy");
                
                if (model.PDF != null)
                {
                    var pathFilePdf = Path.Combine(HttpContext.Current.Server.MapPath(pathUpload), dateDayNow, "Document");

                    db = new BMotionDBEntities();
                    Document docEntity = new Document();
                    docEntity.DocumentNo = model.DocumentNo;
                    docEntity.NIP = model.NIP;
                    docEntity.Quota = model.Quota;
                    docEntity.DocumentFile = model.PDF.FileName;
                    docEntity.ExpDate = Convert.ToDateTime(model.ExpDate);
                    docEntity.IsVerify = "Y";
                    docEntity.CreatedDate = DateTime.Now;
                    docEntity.CreatedBy = SessionManager.NIP();
                    db.Documents.Add(docEntity);
                    db.SaveChanges();

                    if (!Directory.Exists(pathFilePdf))
                    {
                        System.IO.Directory.CreateDirectory(pathFilePdf);
                    }
                    using (var fileStream = new System.IO.FileStream(pathFilePdf + "\\" + model.PDF.FileName, System.IO.FileMode.Create, System.IO.FileAccess.Write))
                    {
                        model.PDF.InputStream.CopyTo(fileStream);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Verification(string documentNo)
        {
            try
            {
                db = new BMotionDBEntities();
                Document doc = (from d in db.Documents
                              where d.DocumentNo.Equals(documentNo)
                              select d).First();
                doc.IsVerify = "Y";
                db.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}