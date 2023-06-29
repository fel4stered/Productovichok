using Aspose.Pdf;
using Aspose.Pdf.Text;
using Microsoft.EntityFrameworkCore;
using ProductovichokBot.Data;
using ProductovichokBot.Data.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductovichokBot
{
    internal class UserService
    {
        internal static readonly ProductovichokContext _context = new ProductovichokContext();

        internal static bool RegistrationCheck(int userid)
        {
            if (_context.Users.SingleOrDefault(x => x.UserId == userid) is not null)
                return true;
            else
                return false;
        }
        internal async static void Registration(int userid, string? nickname, string? name)
        {
            await _context.Users.AddAsync(new User()
            {
                UserId = userid,
                TelegramUserNickname = nickname,
                RoleId = 1,
                FirstName = name,
            });
            await _context.SaveChangesAsync();
        }
        internal static async void Authorization(int userid, int code)
        {
            await _context.Codes.AddAsync(new Code()
            {
                UserId = userid,
                CodeId = code,
            });
            await _context.SaveChangesAsync();
        }
        internal static bool AuthorizationCheck(int userid)
        {
            if (_context.Codes.SingleOrDefaultAsync(x => x.UserId == userid).Result is not null)
                return true;
            else
                return false;
        }
        internal static List<string> CreatePdfCheck(int userId)
        {
            List<string> result = new List<string>();
            _context.Orders.ToList();
            _context.Users.ToList();
            _context.Orderdetails.ToList();
            _context.Products.ToList();
            if (_context.Checks.Any(x => x.UserId == userId))
            {
                var checks = _context.Checks.Where(x => x.UserId == userId);

                foreach(Check check in checks)
                {
                    var orderdetails = check.Order.Orderdetails.ToList();

                    // Инициализировать объект документа
                    Document document = new Document();

                    // Добавить страницу
                    Page page = document.Pages.Add();
                    page.SetPageSize(PageSize.A5.Width, PageSize.A5.Height);

                    var header = new TextFragment("ООО 'Продуктовичок'\n");
                    header.TextState.Font = FontRepository.FindFont("Bahnschrift");
                    header.TextState.FontSize = 14;
                    header.HorizontalAlignment = HorizontalAlignment.Left;
                    page.Paragraphs.Add(header);

                    var subheader = new TextFragment($"Чек к заказу #{check.OrderId}");
                    subheader.TextState.Font = FontRepository.FindFont("Bahnschrift");
                    subheader.TextState.FontSize = 11;
                    subheader.HorizontalAlignment = HorizontalAlignment.Left;
                    page.Paragraphs.Add(subheader);

                    var timeOrder = new TextFragment($"\nДата создания чека:\n{DateTime.Now}\nДата заказа:\n{check.Order.OrderDateTime}");
                    timeOrder.TextState.Font = FontRepository.FindFont("Bahnschrift");
                    timeOrder.TextState.FontSize = 9;
                    timeOrder.HorizontalAlignment = HorizontalAlignment.Left;
                    page.Paragraphs.Add(timeOrder);

                    var user = new TextFragment($"\nЗаказчик: {check.User.TelegramUserNickname}");
                    user.TextState.Font = FontRepository.FindFont("Bahnschrift");
                    user.TextState.FontSize = 9;
                    user.HorizontalAlignment = HorizontalAlignment.Left;
                    page.Paragraphs.Add(user);

                    // Add table
                    var table = new Table
                    {
                        ColumnWidths = "60",
                        Border = new BorderInfo(BorderSide.Box, 0.2f, Aspose.Pdf.Color.DarkSlateGray),
                        DefaultCellBorder = new BorderInfo(BorderSide.Box, 0.5f, Aspose.Pdf.Color.Black),
                        Margin =
                {
                    Bottom = 10
                },
                        DefaultCellTextState =
                {
                    Font =  FontRepository.FindFont("Bahnschrift")
                }
                    };

                    var headerRow = table.Rows.Add();
                    headerRow.Cells.Add("Продукт");
                    headerRow.Cells.Add("Цена");
                    headerRow.Cells.Add("Кол-во");
                    headerRow.Cells.Add("Стоимость");
                    foreach (Cell headerRowCell in headerRow.Cells)
                    {
                        headerRowCell.BackgroundColor = Aspose.Pdf.Color.Gray;
                        headerRowCell.DefaultCellTextState.ForegroundColor = Aspose.Pdf.Color.WhiteSmoke;
                    }

                    table.HorizontalAlignment = HorizontalAlignment.FullJustify;
                    table.DefaultCellTextState.FontSize = 7;

                    foreach (var item in orderdetails)
                    {
                        var dataRow = table.Rows.Add();
                        dataRow.DefaultCellTextState.HorizontalAlignment = HorizontalAlignment.Center;
                        dataRow.Cells.Add(item.Product.ProductName);
                        dataRow.Cells.Add(item.PriceAtOrder.ToString() + " ₽");
                        dataRow.Cells.Add(item.Quantity.ToString());
                        dataRow.Cells.Add((item.PriceAtOrder * item.Quantity).ToString() + " ₽");
                    }

                    page.Paragraphs.Add(table);

                    var price = new TextFragment($"\nИтого: {check.Order.TotalPrice} ₽");
                    price.TextState.Font = FontRepository.FindFont("Bahnschrift");
                    price.TextState.FontSize = 12;
                    price.HorizontalAlignment = HorizontalAlignment.Left;
                    page.Paragraphs.Add(price);

                    // Сохранить PDF 
                    document.Save($"Заказ#{check.OrderId}.pdf");

                    _context.Checks.Remove(check);
                    result.Add($"Заказ#{check.OrderId}.pdf");
                }
                _context.SaveChanges();
                return result;
            }
            else
                return null;
        }
    }
}
