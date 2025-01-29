using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
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
        public async Task EditTaskAsync(int id)
        {
            await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            _context.SaveChanges();
        }
        public async Task<Product> GetProductByIdAsync(int id)
        {
            // Fetch product by ID (replace with actual data access logic)
            return await _context.Products.FindAsync(id);
        }
        public async Task UpdateProductAsync(Product product)
        {
            // Update the product in the database (replace with actual logic)
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
        public async Task<Product> GetProductbyIdAsync(int id)
        {
            // Fetch the product details by id (replace with actual data fetching logic)
            return await _context.Products.FindAsync(id);
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

        //public async Task<byte[]> GeneratePdfAsync(List<Product> products)
        //{
        //    using (var memoryStream = new MemoryStream())
        //    {
        //        var document = new PdfDocument();
        //        var page = document.AddPage();
        //        var graphics = XGraphics.FromPdfPage(page);

        //        // Define fonts
        //        var titleFont = new XFont("Arial", 16, XFontStyle.Bold); // Bold title
        //        var headerFont = new XFont("Arial", 12, XFontStyle.Bold); // Bold headers
        //        var contentFont = new XFont("Arial", 12, XFontStyle.Regular); // Regular text

        //        // Add a title to the PDF
        //        graphics.DrawString("Product List", titleFont, XBrushes.Black, new XPoint(40, 40));

        //        // Add headers (Bold)
        //        graphics.DrawString("Product Name", headerFont, XBrushes.Black, new XPoint(40, 70));
        //        graphics.DrawString("Price", headerFont, XBrushes.Black, new XPoint(200, 70));
        //        graphics.DrawString("Description", headerFont, XBrushes.Black, new XPoint(300, 70));

        //        // Draw a line under headers
        //        graphics.DrawLine(XPens.Black, 40, 75, 500, 75);

        //        int yPosition = 95; // Starting position for product data
        //        foreach (var product in products)
        //        {
        //            // Product data
        //            graphics.DrawString(product.Name, contentFont, XBrushes.Black, new XPoint(40, yPosition));
        //            graphics.DrawString(product.Price.ToString("C"), contentFont, XBrushes.Black, new XPoint(200, yPosition));
        //            graphics.DrawString(product.Description, contentFont, XBrushes.Black, new XPoint(300, yPosition));

        //            yPosition += 20; // Move to the next line
        //        }

        //        document.Save(memoryStream, false);
        //        return memoryStream.ToArray();
        //    }
        //}




        // Method to generate Excel file for products
        //public async Task<byte[]> GenerateExcelAsync(List<Product> products)
        //{
        //    using (var memoryStream = new MemoryStream())
        //    {
        //        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        //        using (var package = new ExcelPackage(memoryStream))
        //        {
        //            var worksheet = package.Workbook.Worksheets.Add("Products");

        //            // Add headers
        //            worksheet.Cells[1, 1].Value = "Product Name";
        //            worksheet.Cells[1, 2].Value = "Price";

        //            // Add product data
        //            int row = 2;
        //            foreach (var product in products)
        //            {
        //                worksheet.Cells[row, 1].Value = product.Name;
        //                worksheet.Cells[row, 2].Value = product.Price;
        //                row++;
        //            }

        //            await package.SaveAsync();
        //        }
        //        return memoryStream.ToArray();
        //    }

        //}
        public async Task<byte[]> GenerateExcelAsync(List<Product> products)
        {
            using (var memoryStream = new MemoryStream())
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var package = new ExcelPackage(memoryStream))
                {
                    var worksheet = package.Workbook.Worksheets.Add("Products");

                    // Add headers and make them bold
                    worksheet.Cells[1, 1].Value = "Product Name";
                    worksheet.Cells[1, 2].Value = "Price";
                    worksheet.Cells[1, 3].Value = "Description"; // Add description header
                    worksheet.Cells[1, 1, 1, 3].Style.Font.Bold = true; // Makes headers bold

                    // Add product data
                    int row = 2;
                    foreach (var product in products)
                    {
                        worksheet.Cells[row, 1].Value = product.Name;
                        worksheet.Cells[row, 2].Value = product.Price;
                        worksheet.Cells[row, 3].Value = product.Description; // Add description data
                        row++;
                    }

                    await package.SaveAsync();
                }
                return memoryStream.ToArray();
            }
        }
        public async Task<Product> GetProductbyImageAsync(int id)
        {
            // Fetch product from database (or any data source)
            var product = await _context.Products.FindAsync(id);

            // If the product is found, assign the appropriate image URL based on the brand
            if (product != null)
            {
                // Example: Set the image URL based on the brand
                switch (product.Brand.ToLower())
                {
                    case "nike":
                        product.ImageUrl = $"/images/nike/{product.Id}.jpg"; // Assuming Nike images are in the /images/nike/ folder
                        break;
                    case "adidas":
                        product.ImageUrl = $"/images/adidas/{product.Id}.jpg"; // Assuming Adidas images are in the /images/adidas/ folder
                        break;
                    case "ndure":
                        product.ImageUrl = $"/images/ndure/{product.Id}.jpg"; // Assuming Ndure images are in the /images/ndure/ folder
                        break;
                    default:
                        product.ImageUrl = $"/images/default/{product.Id}.jpg"; // Default image if brand is not matched
                        break;
                }
            }

            return product;
        }


    }
}
        


    


