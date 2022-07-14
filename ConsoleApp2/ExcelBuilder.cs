using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;

namespace ConsoleApp2
{
    public class ExcelBuilder
    {
        private const int LieuIdx = 1;
        private const int CategorieIdx = 2;
        private const int VilleIdx = 3;
        private const int DatePublicationIdx = 4;
        private const int DateSejourIdx = 5;
        private const int RatingIdx = 6;
        private const int OrigineIdx = 7;
        private const int UserIdx = 8;
        private const int NbAvisIdx = 9;
        private const int LangueIdx = 10;
        private const int TitreAvisIdx = 11;
        private const int AvisIdx = 12;

        public ExcelBuilder()
        {
            
        }

        public void Create(List<AttractionDto> attractions)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(new FileInfo("MyWorkbook.xlsx")))
            {
                var worksheet = package.Workbook.Worksheets.Add("Inventory");
                BuildHeader(worksheet);
                BuildContent(worksheet, attractions);
                package.Save();
            }
        }

        private void BuildContent(ExcelWorksheet worksheet, List<AttractionDto> attractions)
        {
            var lineNumber = 2;
            for (int i = 0; i < attractions.Count; i++)
            {
                var attraction = attractions[i];
                if (attraction.Reviews.Count == 0)
                {
                    AddLine(worksheet, attraction, null, lineNumber);
                    lineNumber++;
                }
                else
                {
                    for (int j = 0; j < attraction.Reviews.Count; j++)
                    {
                        var review = attraction.Reviews[j];
                        AddLine(worksheet, attraction, review, lineNumber);
                        lineNumber++;
                    }
                }
            }
        }

        private static void BuildHeader(ExcelWorksheet worksheet)
        {
            worksheet.Cells[1, LieuIdx].Value = "LIEU";
            worksheet.Cells[1, CategorieIdx].Value = "CATEGORIE";
            worksheet.Cells[1, VilleIdx].Value = "VILLE";
            worksheet.Cells[1, DatePublicationIdx].Value = "DATE PUBLICATION";
            worksheet.Cells[1, DateSejourIdx].Value = "DATE SEJOUR";
            worksheet.Cells[1, RatingIdx].Value = "RATING";
            worksheet.Cells[1, OrigineIdx].Value = "ORIGINE";
            worksheet.Cells[1, UserIdx].Value = "USER";
            worksheet.Cells[1, NbAvisIdx].Value = "NB D'AVIS (USER)";
            worksheet.Cells[1, LangueIdx].Value = "LANGUE";
            worksheet.Cells[1, TitreAvisIdx].Value = "TITRE DE L'AVIS";
            worksheet.Cells[1, AvisIdx].Value = "AVIS";
        }

        private void AddLine(ExcelWorksheet worksheet, AttractionDto attraction, ReviewDto review, int lineNumber)
        {
            worksheet.Cells[lineNumber, LieuIdx].Value = attraction.Place;
            worksheet.Cells[lineNumber, CategorieIdx].Value = "Attraction";
            worksheet.Cells[lineNumber, VilleIdx].Value = attraction.City;
            if (review is null)
                return;
            worksheet.Cells[lineNumber, DatePublicationIdx].Value = review.PublicationDate;
            worksheet.Cells[lineNumber, DateSejourIdx].Value = review.TripDate;
            worksheet.Cells[lineNumber, RatingIdx].Value = review.Rate;
            if (review.User is not null)
            {
                worksheet.Cells[lineNumber, OrigineIdx].Value = review.User.Origin;
                worksheet.Cells[lineNumber, UserIdx].Value = review.User.Name;
                worksheet.Cells[lineNumber, NbAvisIdx].Value = review.User.RateNumber;
            }
            worksheet.Cells[lineNumber, LangueIdx].Value = "";
            worksheet.Cells[lineNumber, TitreAvisIdx].Value = review.Title;
            worksheet.Cells[lineNumber, AvisIdx].Value = review.Content;
        }
    }
}
