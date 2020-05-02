using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using BusinessLogic.Enums;
using BusinessLogic.HelperModels;

namespace BusinessLogic.BusinessLogic
{
    public class SaveToPdf
    {
        public static void CreateDoc(PdfInfo info)
        {
            Document document = new Document();
            DefineStyles(document);
            Section section = document.AddSection();
            Paragraph paragraph = section.AddParagraph(info.Title);
            paragraph.Format.Alignment = ParagraphAlignment.Center;
            paragraph.Style = "NormalTitle";
            foreach (var visit in info.Educations)
            {
                var visitLabel = section.AddParagraph("Курс №" + visit.Id + " от " + visit.DateOfBuying.ToShortDateString());
                visitLabel.Style = "NormalTitle";
                visitLabel.Format.SpaceBefore = "1cm";
                visitLabel.Format.SpaceAfter = "0,25cm";
                var doctorsLabel = section.AddParagraph("Лекции:");
                doctorsLabel.Style = "NormalTitle";
                var doctorTable = document.LastSection.AddTable();
                List<string> headerWidths = new List<string> { "1cm", "3cm", "2cm", "3cm", "3cm","3cm", "2,5cm"};
                foreach (var elem in headerWidths)
                {
                    doctorTable.AddColumn(elem);
                }
                CreateRow(new PdfRowParameters
                {
                    Table = doctorTable,
                    Texts = new List<string> { "№", "Название", "Спец.",  "Время", "Цена", "Количество"},
                    Style = "NormalTitle",
                    ParagraphAlignment = ParagraphAlignment.Center
                });
                int i = 1;
                foreach (var doctor in visit.EducationCourses)
                {
                    CreateRow(new PdfRowParameters
                    {
                        Table = doctorTable,
                        Texts = new List<string> { i.ToString(), doctor.CourseName, doctor.Specialication, (doctor.Duration* doctor.Count).ToString(), (doctor.Cost * doctor.Count).ToString(), doctor.Count.ToString() },
                        Style = "Normal",
                        ParagraphAlignment = ParagraphAlignment.Left
                    });
                    i++;
                }

                CreateRow(new PdfRowParameters
                {
                    Table = doctorTable,
                    Texts = new List<string> { "", "", "","","", "Итого:", visit.FinalCost.ToString() },
                    Style = "Normal",
                    ParagraphAlignment = ParagraphAlignment.Left
                });
                if (visit.Status == EducationStatus.Имеется)
                {
                    CreateRow(new PdfRowParameters
                    {
                        Table = doctorTable,
                        Texts = new List<string> { "", "", "", "", "", "К получению:", visit.FinalCost.ToString() },
                        Style = "Normal",
                        ParagraphAlignment = ParagraphAlignment.Left
                    });
                }
                else
                {
                    CreateRow(new PdfRowParameters
                    {
                        Table = doctorTable,
                        Texts = new List<string> { "", "", "", "", "", "К получению:", visit.LeftSum.ToString() },
                        Style = "Normal",
                        ParagraphAlignment = ParagraphAlignment.Left
                    });
                }
                if (info.Payments[visit.Id].Count == 0)
                {
                    continue;
                }
                var paymentsLabel = section.AddParagraph("Получено:");
                paymentsLabel.Style = "NormalTitle";
                var paymentTable = document.LastSection.AddTable();
                headerWidths = new List<string> { "1cm", "3cm", "3cm", "3cm" };
                foreach (var elem in headerWidths)
                {
                    paymentTable.AddColumn(elem);
                }
                CreateRow(new PdfRowParameters
                {
                    Table = paymentTable,
                    Texts = new List<string> { "№", "Дата", "Сумма" },
                    Style = "NormalTitle",
                    ParagraphAlignment = ParagraphAlignment.Center
                });
                i = 1;
                foreach (var payment in info.Payments[visit.Id])
                {
                    CreateRow(new PdfRowParameters
                    {
                        Table = paymentTable,
                        Texts = new List<string> { i.ToString(), payment.DatePayment.ToString(), payment.Sum.ToString() },
                        Style = "Normal",
                        ParagraphAlignment = ParagraphAlignment.Left
                    });
                    i++;
                }
            }
            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true)
            {
                Document = document
            };
            renderer.RenderDocument();
            renderer.PdfDocument.Save(info.FileName);
        }
        private static void DefineStyles(Document document)
        {
            Style style = document.Styles["Normal"];
            style.Font.Name = "Times New Roman";
            style.Font.Size = 14;
            style = document.Styles.AddStyle("NormalTitle", "Normal");
            style.Font.Bold = true;
        }
        private static void CreateRow(PdfRowParameters rowParameters)
        {
            Row row = rowParameters.Table.AddRow();
            for (int i = 0; i < rowParameters.Texts.Count; ++i)
            {
                FillCell(new PdfCellParameters
                {
                    Cell = row.Cells[i],
                    Text = rowParameters.Texts[i],
                    Style = rowParameters.Style,
                    BorderWidth = 0.5,
                    ParagraphAlignment = rowParameters.ParagraphAlignment
                });
            }
        }
        private static void FillCell(PdfCellParameters cellParameters)
        {
            cellParameters.Cell.AddParagraph(cellParameters.Text);
            if (!string.IsNullOrEmpty(cellParameters.Style))
            {
                cellParameters.Cell.Style = cellParameters.Style;
            }
            cellParameters.Cell.Borders.Left.Width = cellParameters.BorderWidth;
            cellParameters.Cell.Borders.Right.Width = cellParameters.BorderWidth;
            cellParameters.Cell.Borders.Top.Width = cellParameters.BorderWidth;
            cellParameters.Cell.Borders.Bottom.Width = cellParameters.BorderWidth;
            cellParameters.Cell.Format.Alignment = cellParameters.ParagraphAlignment;
            cellParameters.Cell.VerticalAlignment = VerticalAlignment.Center;
        }
    }
}