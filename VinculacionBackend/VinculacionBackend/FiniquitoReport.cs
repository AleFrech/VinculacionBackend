using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Spire.Doc;
using Spire.Doc.Documents;
using VinculacionBackend.Data.Interfaces;
using VinculacionBackend.Interfaces;

namespace VinculacionBackend
{
    public class FiniquitoReport : IDownloadbleFile
    {
        private readonly ITextDocumentServices _textDoucmentServices;
        private readonly IStudentRepository _studentRepository;

        public FiniquitoReport(ITextDocumentServices textDocumentServices, IStudentRepository studentRepository)
        {

            _textDoucmentServices = textDocumentServices;
            _studentRepository = studentRepository;
        }

        public HttpResponseMessage GenerateFiniquitoReport(string accountId)
        {
            var student = _studentRepository.GetByAccountNumber(accountId);
            var doc = _textDoucmentServices.CreaDocument();
            var page1 = _textDoucmentServices.CreatePage(doc);
            var header = _textDoucmentServices.CreateHeader(doc);
            var headerParagraph = _textDoucmentServices.CreateHeaderParagraph(header);
            _textDoucmentServices.AppendPictureToHeader(headerParagraph,Properties.Resources.LaurateLogo,39f,122f,15f,-2f);
            _textDoucmentServices.AppendPictureToHeader(headerParagraph,Properties.Resources.UnitecFullLogo, 51f, 151f, 430f, -12f);
            var footer = _textDoucmentServices.CreaFooter(doc);
            var footerParagraph = _textDoucmentServices.CreateFooterParagraph(footer);
            _textDoucmentServices.AppendTextToFooter(footerParagraph, "      Elaborado y revisado por:\r\n      Andrea Cecilia Orellana Zelaya\r\n      Jefe de Vinculación y Emprendimiento\r\n\r\n", "Segoe UI", 10f);
            var p0 = _textDoucmentServices.CreateParagraph(page1);
            var p0Style = _textDoucmentServices.CreateParagraphStyle(doc, "FiniquitoTitle", "Segoe UI", 14f, true);
            _textDoucmentServices.AddTextToParagraph("\r\n\r\n\r\n                  CARTA DE FINIQUITO DEFINITIVO DE SERVICIO SOCIAL",p0,p0Style,doc,HorizontalAlignment.Left, 13.8f);
            var p1 = _textDoucmentServices.CreateParagraph(page1);
            _textDoucmentServices.AddTextToParagraph("\r\n\r\n\r\n     Fecha: "+DateTime.Now.Day+" de "+ DateTimeFormatInfo.CurrentInfo.GetMonthName(DateTime.Now.Month)+" del "+DateTime.Now.Year,p1,p0Style,doc, HorizontalAlignment.Left, 13.8f);
            var p2 = _textDoucmentServices.CreateParagraph(page1);
            var p2Style = _textDoucmentServices.CreateParagraphStyle(doc, "FiniquitoBody", "Segoe UI", 12f, false);
            var bodyline1 = "      Yo, Rafael Antonio Delgado Elvir, Director de Desarrollo Institucional de UNITEC";
            var bodyline2 = "      Campus SPS, hago constar que en los registros figura que el estudiante";
            var bodyline3 = "      "+student.Name+", con número de cuenta: "+accountId+", estudiante de la carrera de ";
            var bodyline4 = "      \""+student.Major.Name+"\", completó con todas las ";
            var bodyline5 = "      horas referentes a su Programa de Servicio Social.";
            _textDoucmentServices.AddTextToParagraph("\r\n"+ bodyline1, p2,p2Style,doc,HorizontalAlignment.Justify, 13.8f);
            _textDoucmentServices.AddTextToParagraph("\r\n" + bodyline2, p2, p2Style, doc, HorizontalAlignment.Justify, 13.8f);
            _textDoucmentServices.AddTextToParagraph("\r\n" + bodyline3, p2, p2Style, doc, HorizontalAlignment.Justify, 13.8f);
            _textDoucmentServices.AddTextToParagraph("\r\n" + bodyline4, p2, p2Style, doc, HorizontalAlignment.Justify, 13.8f);
            _textDoucmentServices.AddTextToParagraph("\r\n" + bodyline5, p2, p2Style, doc, HorizontalAlignment.Justify, 13.8f);
            var p3 = _textDoucmentServices.CreateParagraph(page1);
            _textDoucmentServices.AddTextToParagraph("\r\n      El número total de horas de trabajo fue de: "+_studentRepository.GetStudentHours(accountId)+" Horas.",p3,p2Style,doc,HorizontalAlignment.Justify, 13.8f);
            var p4 = _textDoucmentServices.CreateParagraph(page1);
            _textDoucmentServices.AddTextToParagraph("\r\n      Se extiende la presente constancia para los fines que al interesado convengan.",p4,p2Style,doc,HorizontalAlignment.Justify, 13.8f);
            var endingline1 = "\r\n      ______________________________________";
            var endingline2 = "\r\n      Director de Desarrollo Institucional";
            var endingline3 = "\r\n      UNITEC San Pedro Sula";
            var p5 = _textDoucmentServices.CreateParagraph(page1);
            _textDoucmentServices.AddTextToParagraph("\r\n\r\n"+endingline1, p5, p2Style, doc, HorizontalAlignment.Justify, 13.8f);
            _textDoucmentServices.AddTextToParagraph(endingline2, p5, p2Style, doc, HorizontalAlignment.Justify, 13.8f);
            _textDoucmentServices.AddTextToParagraph(endingline3, p5, p2Style, doc, HorizontalAlignment.Justify, 13.8f);
            return ToHttpResponseMessage(doc);
        }


        public HttpResponseMessage ToHttpResponseMessage(Document document)
        {
            var ms = new MemoryStream();
            document.SaveToStream(ms, FileFormat.Docx);
            ms.Position = 0;
            var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StreamContent(ms) };
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "Finiquito.docx"
            };
            return response;
        }
    }
}