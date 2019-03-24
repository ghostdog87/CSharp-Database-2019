namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    public class StartUp
    {
        public static void Main()
        {
            using (var db = new BookShopContext())
            {
                int result = RemoveBooks(db);

                Console.WriteLine(result);
            }
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            AgeRestriction ageRestriction = Enum.Parse<AgeRestriction>(command, true);
            var books = context
                .Books
                .Where(x => x.AgeRestriction == ageRestriction)
                .Select(x => x.Title)
                .OrderBy(x => x)
                .ToList();

            var result = string.Join(Environment.NewLine, books);
            return result;
        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            var books = context
                .Books
                .Where(x => x.EditionType == EditionType.Gold && x.Copies < 5000)
                .OrderBy(x => x.BookId)
                .Select(x => x.Title)                
                .ToList();

            var result = string.Join(Environment.NewLine, books);
            return result;
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            var books = context
                .Books
                .Where(x => x.Price >
 40)
                .OrderByDescending(x => x.Price)
                .Select(x => new {
                    x.Title,
                    x.Price
                })
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - ${book.Price:F2}");
            }
            return sb.ToString().TrimEnd();
        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var books = context
                .Books
                .Where(x => x.ReleaseDate.Value.Year != year)
                .OrderBy(x => x.BookId)
                .Select(x => x.Title)
                .ToList();

            var result = string.Join(Environment.NewLine, books);
            return result;
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            var categories = input.ToLower().Split(" ", StringSplitOptions.RemoveEmptyEntries);

            var books = context
                .Books
                .Where(bc =>bc.BookCategories.Any(c => categories.Contains(c.Category.Name.ToLower())))          
                .Select(x => x.Title)
                .OrderBy(x => x)
                .ToList();

            var result = string.Join(Environment.NewLine, books);
            return result;
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            DateTime formatedDate = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var books = context
                .Books
                .Where(d => d.ReleaseDate < formatedDate)
                .OrderByDescending(x => x.ReleaseDate)
                .Select(x => new
                {
                    x.Title,
                    x.EditionType,
                    x.Price
                })
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - {book.EditionType} - ${book.Price:F2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authors = context
                .Authors
                .Where(x => EF.Functions.Like(x.FirstName, "%" + input))
                .Select(x => new
                {
                    x.FirstName,
                    x.LastName
                })
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .ToList();


            StringBuilder sb = new StringBuilder();

            foreach (var author in authors)
            {
                sb.AppendLine($"{author.FirstName} {author.LastName}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var books = context
                .Books
                .Where(x => EF.Functions.Like(x.Title.ToLower(), "%" + input + "%"))
                .Select(x => x.Title)
                .OrderBy(x => x)
                .ToList();


            var result = string.Join(Environment.NewLine, books);
            return result;
        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var authors = context
                .Authors
                .Where(x => EF.Functions.Like(x.LastName, input + "%"))
                .Select(x => new
                {
                    x.FirstName,
                    x.LastName,
                    books = x.Books.Select(y => new {
                        y.BookId,
                        y.Title
                    })
                    .OrderBy(z => z.BookId)
                    .ToList()
                })               
                .ToList();


            StringBuilder sb = new StringBuilder();

            foreach (var author in authors)
            {
                foreach (var book in author.books)
                {
                    sb.AppendLine($"{book.Title} ({author.FirstName + " " + author.LastName})");
                }
                
            }

            return sb.ToString().TrimEnd();
        }

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var books = context
                .Books
                .Count(x => x.Title.Length > lengthCheck);

            return books;
        }

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var bookCopies = context
                .Authors
                .Select(x => new
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Copies = x.Books.Sum(s => s.Copies)
                })
                .OrderByDescending(x => x.Copies)
                .ToList();

            string result = string.Join(Environment.NewLine, bookCopies.Select(x => x.FirstName + " " + x.LastName + " - " + x.Copies));

            return result;
        }

        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var categories = context
                .Categories
                .Select(x => new
                {
                    Category = x.Name,
                    BookProfit = x.CategoryBooks.Sum(e => e.Book.Copies * e.Book.Price)
                })
                .OrderByDescending(x => x.BookProfit)
                .ThenBy(x => x.Category)
                .ToList();

            string result = string.Join(Environment.NewLine, categories.Select(x => x.Category + " $" + x.BookProfit));

            return result;
        }

        public static string GetMostRecentBooks(BookShopContext context)
        {
            var categories = context
                .Categories
                .Select(x => new
                {
                    Name = x.Name,
                    Books = x.CategoryBooks.Select(b => new
                    {
                        b.Book.ReleaseDate,
                        b.Book.Title
                    })
                    .OrderByDescending(r => r.ReleaseDate)
                    .ToList()
                })
                .OrderBy(x => x.Name)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var category in categories)
            {
                sb.AppendLine($"--{category.Name}");
                foreach (var books in category.Books.Take(3))
                {
                    sb.AppendLine($"{books.Title} ({books.ReleaseDate.Value.Year})");
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static void IncreasePrices(BookShopContext context)
        {
            var books = context
                .Books
                .Where(x => x.ReleaseDate.Value.Year < 2010)
                .ToList();

            foreach (var book in books)
            {
                book.Price += 5;
            }

            context.SaveChanges();
        }

        public static int RemoveBooks(BookShopContext context)
        {
            var books = context
                .Books
                .Where(x => x.Copies < 4200)
                .ToList();

            context.Books.RemoveRange(books);
            context.SaveChanges();
            return books.Count;
        }
    }
}
