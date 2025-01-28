using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Pagination.Models;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Reflection.Metadata;
using static System.Net.Mime.MediaTypeNames;

namespace Pagination.Services
{
    public class ProductServices
    {
        private readonly ApplicationDbContext _context;

        public ProductServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var pr = await _context.Products.FindAsync(id);
            _context.Remove(pr);
            _context.SaveChangesAsync();

        }

        public async Task<(List<Product>, int)> GetPaginatedProductsAsync(int pageNumber, int pageSize)
        {
            var totalItems = await _context.Products.CountAsync();
            var products = await _context.Products
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (products, totalItems);
        }


        public async Task<byte[]> GeneratePdfAsync(List<Product> products)
        {
            using (var memoryStream = new MemoryStream())
            {
                var document = new PdfDocument();
                var page = document.AddPage();
                var graphics = XGraphics.FromPdfPage(page);
                var font = new XFont("Arial", 12);
                

                // Add a title to the PDF
                graphics.DrawString("Product List", font, XBrushes.Black, new XPoint(40, 40));

                // Add headers
                graphics.DrawString("Product Name", font, XBrushes.Black, new XPoint(40, 60));
                graphics.DrawString("Price", font, XBrushes.Black, new XPoint(200, 60));

                int yPosition = 80;
                foreach (var product in products)
                {
                    graphics.DrawString(product.Name, font, XBrushes.Black, new XPoint(40, yPosition));
                    graphics.DrawString(product.Price.ToString("C"), font, XBrushes.Black, new XPoint(200, yPosition));
                    yPosition += 20; // Move to the next line
                }

                document.Save(memoryStream, false);
                return memoryStream.ToArray();
            }
        }

        // Method to generate Excel file for products
        public async Task<byte[]> GenerateExcelAsync(List<Product> products)
        {
            using (var memoryStream = new MemoryStream())
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var package = new ExcelPackage(memoryStream))
                {
                    var worksheet = package.Workbook.Worksheets.Add("Products");

                    // Add headers
                    worksheet.Cells[1, 1].Value = "Product Name";
                    worksheet.Cells[1, 2].Value = "Price";

                    // Add product data
                    int row = 2;
                    foreach (var product in products)
                    {
                        worksheet.Cells[row, 1].Value = product.Name;
                        worksheet.Cells[row, 2].Value = product.Price;
                        row++;
                    }

                    await package.SaveAsync();
                }
                return memoryStream.ToArray();
            }
        }
    }
}
        


    


