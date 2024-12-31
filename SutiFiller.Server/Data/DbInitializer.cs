using System.Security.Cryptography;
using System.Text;

namespace SutiFiller.Server.Data
{
    public class DbInitializer
    {
        public static void Initialize(SutisContext context, string imageDirectory)
        {
            context.Database.EnsureCreated();

            if (context.Categories.Any())
            {
                return;
            }

            SeedCategories(context);
            SeedStatuses(context);
            SeedSutis(context);
            SeedOrders(context);
            SeedSutiOrders(context);
            //SeedGuests(context);
            SeedImages(imageDirectory, context);
        }

        private static void SeedCategories(SutisContext context)
        {

            var categories = new Category[]
            {
                new Category {Name = "Suti"},
                new Category {Name = "Torta"},
                new Category {Name = "Keksz"}
            };
            foreach (Category c in categories)
            {
                context.Categories.Add(c);
            }

            context.SaveChanges();
        }

        private static void SeedStatuses(SutisContext context)
        {

            var statuses = new Status[]
            {
                new Status {Name = "Ordered"},
                new Status {Name = "Done"},
                new Status {Name = "Cancelled"},
                new Status {Name = "Failed"},
                new Status {Name = "NoStatus"},
            };
            foreach (Status s in statuses)
            {
                context.Statuses.Add(s);
            }

            context.SaveChanges();
        }

        private static void SeedSutis(SutisContext context)
        {
            var sutik = new Suti[]
            {
                new Suti {
                    Name = "Kek Suti", 
                    CategoryId = 1, 
                    Description = "2 kek suti komment",
                    Price = 700,
                },
                new Suti {
                    Name = "Piros Suti", 
                    CategoryId = 1, 
                    Description = "egy piros suti komment",
                    Price = 800,
                },
                new Suti {
                    Name = "Muffin", 
                    CategoryId = 1, 
                    Description = "Különböző ízek",
                    Price = 650,
                },
                new Suti {
                    Name = "Citromos Sajttorta Nagy", 
                    CategoryId = 2, 
                    Description = "Hagyományos v mentes. 12 szeletes",
                    Price = 14400,
                },
                new Suti {
                    Name = "Citromos Sajttorta Kicsi",
                    CategoryId = 2,
                    Description = "Hagyományos v mentes. 8 szeletes",
                    Price = 9600,
                },
                new Suti {
                    Name = "Citromos Sajttorta Mini",
                    CategoryId = 2,
                    Description = "Hagyományos v mentes. 4 szeletes",
                    Price = 4800,
                },
                new Suti {
                    Name = "Macaron", 
                    CategoryId = 3, 
                    Description = "Különböző színek, ízek",
                    Price = 350,
                },
                new Suti {
                    Name = "Linzer",
                    CategoryId = 3,
                    Description = "Különböző ízvariációk.",
                    Price = 250,
                },
            };
            foreach (Suti s in sutik)
            {
                context.Sutis.Add(s);
            }

            context.SaveChanges();
        }

        private static void SeedImages(string imageDirectory, SutisContext context)
        {
            if (Directory.Exists(imageDirectory))
            {
                var images = new List<Image>();

                var largePath = Path.Combine(imageDirectory, "kek_suti.jpg");
                var smallPath = Path.Combine(imageDirectory, "kek_suti_thumb.jpg");
                if (File.Exists(largePath) && File.Exists(smallPath))
                {
                    images.Add(new Image
                    {
                        SutiId = 1,
                        ImageLarge = File.ReadAllBytes(largePath),
                        ImageSmall = File.ReadAllBytes(smallPath)
                    });
                }

                largePath = Path.Combine(imageDirectory, "piros_suti.jpg");
                smallPath = Path.Combine(imageDirectory, "piros_suti_thumb.jpg");
                if (File.Exists(largePath) && File.Exists(smallPath))
                {
                    images.Add(new Image
                    {
                        SutiId = 2,
                        ImageLarge = File.ReadAllBytes(largePath),
                        ImageSmall = File.ReadAllBytes(smallPath)
                    });
                }

                largePath = Path.Combine(imageDirectory, "muffin.jpg");
                smallPath = Path.Combine(imageDirectory, "muffin_thumb.jpg");
                if (File.Exists(largePath) && File.Exists(smallPath))
                {
                    images.Add(new Image
                    {
                        SutiId = 3,
                        ImageLarge = File.ReadAllBytes(largePath),
                        ImageSmall = File.ReadAllBytes(smallPath)
                    });
                }

                largePath = Path.Combine(imageDirectory, "sajttorta.jpg");
                smallPath = Path.Combine(imageDirectory, "sajttorta_thumb.jpg");
                if (File.Exists(largePath) && File.Exists(smallPath))
                {
                    images.Add(new Image
                    {
                        SutiId = 4,
                        ImageLarge = File.ReadAllBytes  (largePath),
                        ImageSmall = File.ReadAllBytes(smallPath)
                    });
                }

                largePath = Path.Combine(imageDirectory, "sajttorta.jpg");
                smallPath = Path.Combine(imageDirectory, "sajttorta_thumb.jpg");
                if (File.Exists(largePath) && File.Exists(smallPath))
                {
                    images.Add(new Image
                    {
                        SutiId = 5,
                        ImageLarge = File.ReadAllBytes(largePath),
                        ImageSmall = File.ReadAllBytes(smallPath)
                    });
                }

                largePath = Path.Combine(imageDirectory, "sajttorta.jpg");
                smallPath = Path.Combine(imageDirectory, "sajttorta_thumb.jpg");
                if (File.Exists(largePath) && File.Exists(smallPath))
                {
                    images.Add(new Image
                    {
                        SutiId = 6,
                        ImageLarge = File.ReadAllBytes(largePath),
                        ImageSmall = File.ReadAllBytes(smallPath)
                    });
                }

                largePath = Path.Combine(imageDirectory, "macaron.jpg");
                smallPath = Path.Combine(imageDirectory, "macaron_thumb.jpg");
                if (File.Exists(largePath) && File.Exists(smallPath))
                {
                    images.Add(new Image
                    {
                        SutiId = 7,
                        ImageLarge = File.ReadAllBytes(largePath),
                        ImageSmall = File.ReadAllBytes(smallPath)
                    });
                }

                largePath = Path.Combine(imageDirectory, "linzer.jpg");
                smallPath = Path.Combine(imageDirectory, "linzer_thumb.jpg");
                if (File.Exists(largePath) && File.Exists(smallPath))
                {
                    images.Add(new Image
                    {
                        SutiId = 8,
                        ImageLarge = File.ReadAllBytes(largePath),
                        ImageSmall = File.ReadAllBytes(smallPath)
                    });
                }

                foreach (var image in images)
                {
                    context.Images.Add(image);
                }

                context.SaveChanges();
            }
        }

        private static void SeedOrders(SutisContext context)
        {
            var orders = new Order[]
            {
                new Order {
                    Name = "Marika",
                    CustomerId = 1,
                    BillingAddress = "marikaneni cimere kuldom",
                    PhoneNumber = "0123456789",
                    StatusId = 1,
                    DueDate = new DateTime(2024, 10, 10),
                    PrePayment = 2200,
                    TotalPrice = 24000,
                    Comment = "A macaronokat pirosra festve szeretném és finomra. A macaronokat pirosra festve szeretném és finomra. A macaronokat pirosra festve szeretném és finomra. ",

                },
                new Order {
                    Name = "Peti",
                    CustomerId = 2,
                    BillingAddress = "petibacsi cimere kuldom",
                    PhoneNumber = "9876543210",
                    StatusId = 2,
                    DueDate = new DateTime(2024, 11, 10),
                    PrePayment = 28800,
                    TotalPrice = 360000,
                    Comment = "A tortakat kekre festve szeretném és finomra.",

                },
                new Order {
                    Name = "Antal",
                    CustomerId = 3,
                    BillingAddress = "antal cimereeeeeeeeee kuldom",
                    PhoneNumber = "0123456789",
                    StatusId = 3,
                    DueDate = new DateTime(2024, 12, 10),
                    PrePayment = 2200,
                    TotalPrice = 25000,
                    Comment = "A macaronokat A macaronokat A macaronokatA macaronokatA macaronokatA macaronokatA macaronokat pirosra festve szeretném és finomra. A macaronokat pirosra festve szeretném és finomra. A macaronokat pirosra festve szeretném és finomra. ",
                },
                new Order {
                    Name = "Antal",
                    CustomerId = 3,
                    BillingAddress = "antal cimereeeeeeeeee kuldom",
                    PhoneNumber = "0123456789",
                    StatusId = 5,
                    DueDate = new DateTime(2024, 1, 10),
                    PrePayment = 2200,
                    TotalPrice = 25000,
                    Comment = "Aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa macaronokat A macaronokat A macaronokatA macaronokatA macaronokatA macaronokatA macaronokat pirosra festve szeretném és finomra. A macaronokat pirosra festve szeretném és finomra. A macaronokat pirosra festve szeretném és finomra. ",
                },
                new Order {
                    Name = "Antalka",
                    CustomerId = 3,
                    BillingAddress = "antal cimereeeeeeeeee kuldom",
                    PhoneNumber = "0123456789",
                    StatusId = 4,
                    DueDate = new DateTime(2024, 10, 10),
                    PrePayment = 2200,
                    TotalPrice = 2000,
                    Comment = "Aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa macaronokat A macaronokat A macaronokatA macaronokatA macaronokatA macaronokatA macaronokat pirosra festve szeretném és finomra. A macaronokat pirosra festve szeretném és finomra. A macaronokat pirosra festve szeretném és finomra. ",
                },
                new Order {
                    Name = "Antal",
                    CustomerId = 3,
                    BillingAddress = "antal cimereeeeeeeeee kuldom",
                    PhoneNumber = "0123456789",
                    StatusId = 1,
                    DueDate = new DateTime(2023, 9, 10),
                    PrePayment = 2200,
                    TotalPrice = 29000,
                    Comment = "A macaronokat A macaronokat A macaronokatA macaronokatA macaronokatA macaronokatA macaronokat pirosra festve szeretném és finomra. A macaronokat pirosra festve szeretném és finomra. A macaronokat pirosra festve szeretném és finomra. ",
                },
            };
            foreach (Order o in orders)
            {
                context.Orders.Add(o);
            }

            context.SaveChanges();
        }

        private static void SeedGuests(SutisContext context)
        {

            Byte[] passwordBytes;
            using (var alg = SHA512.Create())
            {
                passwordBytes = alg.ComputeHash(Encoding.UTF8.GetBytes("suti1234"));
            }

            var guests = new Guest[]
            {
                new Guest {
                    Id = 1,
                    Name = "SutiRobert",
                    PhoneNumber = "0123456789",
                    Address = "SutiCíme",
                    UserName = "suti",
                    UserPassword = passwordBytes,
                },
            };
            foreach (Guest g in guests)
            {
                context.Guests.Add(g);
            }

            context.SaveChanges();
        }

        private static void SeedSutiOrders(SutisContext context)
        {
            var sutiorders = new SutiOrder[]
            {
                new SutiOrder {
                    OrderId = 1,
                    SutiId = 1,
                    Message = "Rendelni kell még kék festéket meg sütilisztet.",
                    Quantity = 1,
                    AllInPrice = 700,
                },
                new SutiOrder {
                    OrderId = 1,
                    SutiId = 2,
                    Message = "3ra van alapanyag, többit vásárolni kell.",
                    Quantity = 1,
                    AllInPrice = 800,
                },
                new SutiOrder {
                    OrderId = 1,
                    SutiId = 3,
                    Message = "3ra van alapanyag, többit vásárolni kell.",
                    Quantity = 2,
                    AllInPrice = 650,
                },
                new SutiOrder {
                    OrderId = 1,
                    SutiId = 4,
                    Message = "Ebből 10et leet egyszerre csinálni max.",
                    Quantity = 2,
                    AllInPrice = 28800,
                },
                new SutiOrder {
                    OrderId = 1,
                    SutiId = 5,
                    Message = "Ebből 10et leet egyszerre csinálni max.",
                    Quantity = 2,
                    AllInPrice = 19200,
                },
                new SutiOrder {
                    OrderId = 1,
                    SutiId = 6,
                    Message = "1tepsiben 40 darab férel.",
                    Quantity = 1,
                    AllInPrice = 4800,
                },
                new SutiOrder {
                    OrderId = 1,
                    SutiId = 7,
                    Message = "1tepsiben 40 darab férel.",
                    Quantity = 4,
                    AllInPrice = 1400,
                },
                new SutiOrder {
                    OrderId = 1,
                    SutiId = 8,
                    Message = "Rendelni kell még kék festéket meg sütilisztet.",
                    Quantity = 1,
                    AllInPrice = 250,
                },
                new SutiOrder {
                    OrderId = 2,
                    SutiId = 1,
                    Message = "3ra van alapanyag, többit vásárolni kell.",
                    Quantity = 6,
                    AllInPrice = 4200,
                },
                new SutiOrder {
                    OrderId = 2,
                    SutiId = 3,
                    Message = "1tepsiben 40 darab férel.",
                    Quantity = 2,
                    AllInPrice = 1300,
                },
                new SutiOrder {
                    OrderId = 2,
                    SutiId = 4,
                    Message = "Rendelni kell még kék festéket meg sütilisztet.",
                    Quantity = 4,
                    AllInPrice = 57600,
                },
                new SutiOrder {
                    OrderId = 2,
                    SutiId = 5,
                    Message = "3ra van alapanyag, többit vásárolni kell.",
                    Quantity = 1,
                    AllInPrice = 9600,
                },
                new SutiOrder {
                    OrderId = 2,
                    SutiId = 6,
                    Message = "1tepsiben 40 darab férel.",
                    Quantity = 5,
                    AllInPrice = 24000,
                },
                new SutiOrder {
                    OrderId = 2,
                    SutiId = 7,
                    Message = "Rendelni kell még kék festéket meg sütilisztet.",
                    Quantity = 4,
                    AllInPrice = 3200,
                },
                new SutiOrder {
                    OrderId = 2,
                    SutiId = 8,
                    Message = "3ra van alapanyag, többit vásárolni kell.",
                    Quantity = 1,
                    AllInPrice = 250,
                },
                new SutiOrder {
                    OrderId = 3,
                    SutiId = 4,
                    Message = "Rendelni kell még kék festéket meg sütilisztet.",
                    Quantity = 1,
                    AllInPrice = 650,
                },
                new SutiOrder {
                    OrderId = 3,
                    SutiId = 5,
                    Message = "Rendelni kell még kék festéket meg sütilisztet.",
                    Quantity = 3,
                    AllInPrice = 1950,
                },
                new SutiOrder {
                    OrderId = 3,
                    SutiId = 6,
                    Message = "3ra van alapanyag, többit vásárolni kell.",
                    Quantity = 1,
                    AllInPrice = 4800,
                },
                new SutiOrder {
                    OrderId = 3,
                    SutiId = 7,
                    Message = "1tepsiben 40 darab férel.",
                    Quantity = 10,
                    AllInPrice = 3500,
                },
                new SutiOrder {
                    OrderId = 3,
                    SutiId = 8,
                    Message = "Rendelni kell még kék festéket meg sütilisztet.",
                    Quantity = 5,
                    AllInPrice = 1250,
                },
                new SutiOrder {
                    OrderId = 3,
                    SutiId = 8,
                    Message = "1tepsiben 40 darab férel.",
                    Quantity = 2,
                    AllInPrice = 9600,
                },
            };
            foreach (SutiOrder so in sutiorders)
            {
                context.SutiOrders.Add(so);
            }

            context.SaveChanges();
        }
    }
}
