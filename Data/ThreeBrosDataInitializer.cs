using Microsoft.AspNetCore.Identity;
using Shop.Models.Domain;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Shop.Models;
using Shop.Models.Domain.Enum;

namespace Shop.Data
{
    public class ThreeBrosDataInitializer
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public ThreeBrosDataInitializer(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task InitializeData()
        {
            _dbContext.Database.EnsureDeleted();
            if (_dbContext.Database.EnsureCreated())
            {
                Category converse = new Category("converse", "fa-star");
                Category nike = new Category("nike", "fa-star");
                Category adidas = new Category("adidas", "fa-star");
                Category palladium = new Category("palladium", "fa-star");
                Category vans = new Category("vans", "fa-star");

                _dbContext.Category.AddRange(converse, vans, nike, adidas, palladium);

                Seller Seller01 = new Seller("1970s Base Camp Suede", "lekker@gmail.com", "0123456789", "Vải lưới ở lưỡi gà và dây giày giúp sản phẩm trở nên nổi bật hơn", "Arbeidstraat", "14", "9300", "Aalst", true);
                Seller Seller02 = new Seller("1970s Vintage Canvas", "Itemstinck@gmail.com", "0123456789", "Vẫn là màu vàng best-seller của nhà Converse.", "Paepestraat", "178", "9260", "Wichelen", true);
                Seller Seller03 = new Seller("1970s Vintage Canvas", "schets@gmail.com", "0123456789", "Tone màu xanh lạ mắt, dễ chịu nhưng không kém phần cá tính.", "Ravensteinstraat", "50", "1000", "Brussel", true);
                Seller Seller04 = new Seller("Chuck Taylor All Star 1970s", "coninck@gmail.com", "0123456789", "Thiết kế cổ điển của Chuck 1970s với tone đen huyền thoại ", "Sint-Pietersnieuwstraat", "124", "9000", "Gent", true);
                Seller Seller05 = new Seller("Chuck Taylor All Star 1970s", "Seller01@gmail.com", "0123456789", "Phiên bản cổ thấp màu đen cá tính của Chuck 1970s được giới trẻ nhiệt tình ưu ái", "Arbeidstraat", "14", "9300", "Aalst", true);
                Seller Seller06 = new Seller("1970s Base Camp Suede", "sandwich@gmail.com", "0123456789", "Vải lưới ở lưỡi gà và dây giày giúp sản phẩm trở nên nổi bật hơn.", "Arbeidstraat", "14", "9300", "Aalst", true);
                Seller Seller07 = new Seller("McDonalds", "mc@gmail.com", "0123456789", "Voor een snelle hap moet u bij ons zijn!", "Arbeidstraat", "14", "9300", "Aalst", true);
                Seller Seller08 = new Seller("SOS Piet", "sospiet@gmail.com", "0123456789", "Het echte restaurant van SOS Piet. Altijd de beste maatlijd voor een gezonde prijs!", "Paepestraat", "178", "9260", "Wichelen", true);
                Seller Seller09 = new Seller("CoBoSh", "cobosh@gmail.com", "0123456789", "Voor de beste wijnen moet je bij ons zijn! Hierbij kan altijd een hapje geserveerd worden.", "Arbeidstraat", "14", "9300", "Aalst", true);

                Seller Seller11 = new Seller("Sanitas", "sanitas@gmail.com", "0123456789", "Bekenste nike van Wichelen.", "Paepestraat", "178", "9260", "Wichelen", true);
                Seller Seller12 = new Seller("nike Basic-Fit", "basicfit@gmail.com", "0123456789", "Bekenste nike van België met vestigingen over het hele land.", "Ravensteinstraat", "50", "1000", "Brussel", true);


                _dbContext.Seller.AddRange(Seller01, Seller02, Seller03, Seller04, Seller05, Seller06, Seller07, Seller08, Seller09, Seller12, Seller11);

                _dbContext.SaveChanges();

                _dbContext.User.Add(new User
                {
                    EmailAddress = "khach3brosshop@gmail.com",
                    FirstName = "Dung",
                    FamilyName = "Nguoi",
                    PhoneNumber = "0317258529",
                    Address = "long son, p ky hoa, q tan quy, tp sam son",
                    Sex = Sex.Male
                });
                //admin example
                await CreateUser("quocbao0505.qt@gmail.com", "quocbao0505.qt@gmail.com", "admin123456", "admin");
                await CreateUser("longvox98@gmail.com", "longvox98@gmail.com", "adminadmin", "admin");
                await CreateUser("nguoidung@gmail.com", "nguoidung@gmail.com", "nguoidung", "customers");
                _dbContext.User.Add(new User
                {
                    EmailAddress = "quocbao0505.qt@gmail.com",
                    FirstName = "Bao",
                    FamilyName = "Quoc",
                    PhoneNumber = "0317258529",
                    Address = "long son, p ky hoa, q tan quy, tp quan tri",
                    Sex = Sex.Male
                });
                _dbContext.User.Add(new User
                {
                    EmailAddress = "longvox98@gmail.com",
                    FirstName = "Long",
                    FamilyName = "Hoang",
                    PhoneNumber = "0317258529",
                    Address = "long son, p ky hoa, q tan quy, tp hue",
                    Sex = Sex.Male
                });
                _dbContext.User.Add(new User
                {
                    EmailAddress = "nguoidung@gmail.com",
                    FirstName = "Dung",
                    FamilyName = "Nguoi",
                    PhoneNumber = "0317258529",
                    Address = "long son, p ky hoa, q tan quy, tp sam son",
                    Sex = Sex.Male
                });

                _dbContext.SaveChanges();

                //seller example
                var user = new ApplicationUser { UserName = "3bros.suport@gmail.com", Email = "3bros.suport@gmail.com", EmailConfirmed = true };
                var password = "banhang";
                var result = await _userManager.CreateAsync(user, password);

                _dbContext.User.Add(new User
                {
                    EmailAddress = "3bros.suport@gmail.com",
                    FirstName = "Ban",
                    FamilyName = "Hang",
                    PhoneNumber = "0879654123",
                    Address = "tan hanh, p phu quy, q son lam, tp huong tra",
                    Sex = Sex.Male
                });
                await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "seller"));

                Seller localMarket = new Seller("BanHang", "3bros.suport@gmail.com", "0123456789", "abc", "Denderstraat", "22", "9300", "Aalst", true);
                _dbContext.Seller.Add(localMarket);

                Items localMarketItem = new Items("Lokaalmarkt Aalst", 15, "De beste markt van aalst!", 27, @"images\items\71\", converse, "Denderstraat", "22", "9300", "Aalst", localMarket, Offer.No, true);
                _dbContext.Items.Add(localMarketItem);

                Items Items01 = new Items("1970s Base Camp Suede", 1700000, "Vải lưới ở lưỡi gà và dây giày giúp sản phẩm trở nên nổi bật hơn", 17, @"images\items\1\", converse, "Sư Vạn Hạnh", "14", "9300", "Hồ Chí Minh", localMarket, Offer.No, true);
                Items Items02 = new Items("1970s Vintage Canvas", 1700000, "Vẫn là màu vàng best-seller của nhà Converse.", 242, @"images\items\2\", converse, "Paepestraat", "178", "9260", "Wichelen", localMarket, Offer.Slider, true);
                Items Items03 = new Items("1970s Vintage Canvas", 1700000, "Tone màu xanh lạ mắt, dễ chịu nhưng không kém phần cá tính.", 42, @"images\items\3\", converse, "Ravensteinstraat", "50", "1000", "Brussel", localMarket, Offer.No, true);
                Items Items04 = new Items("Chuck Taylor All Star 1970s", 1800000, "Thiết kế cổ điển của Chuck 1970s với tone đen huyền thoại .", 24, @"images\items\4\", converse, "Sint-Pietersnieuwstraat", "124", "9000", "Gent", localMarket, Offer.No, true);
                Items Items05 = new Items("Chuck Taylor All Star 1970s", 1400000, "Phiên bản cổ thấp màu đen cá tính của Chuck 1970s được giới trẻ nhiệt tình ưu ái.", 124, @"images\items\5\", converse, "Arbeidstraat", "14", "9300", "Aalst", localMarket, Offer.Slider, true);
                Items Items06 = new Items("1970s Base Camp Suede", 1500000, "Vải lưới ở lưỡi gà và dây giày giúp sản phẩm trở nên nổi bật hơn.", 45, @"images\items\6\", converse, "Arbeidstraat", "14", "9300", "Aalst", localMarket, Offer.No, true);
                Items Items07 = new Items("Chuck Taylor All Star", 1300000, "Với 2 tone màu Xanh Navy và Xanh Chàm tươi mát cùng xuất hiện ngay trên đôi giày sẽ giúp bạn có được một sản phẩm không bao giờ lỗi mốt.", 98, @"images\items\7\", converse, "Arbeidstraat", "14", "9300", "Aalst", localMarket, Offer.No, true);
                Items Items08 = new Items("All Star Limo Leather", 1700000, "Toàn bộ thân giày được làm từ chất liệu da cao cấp cùng tone Xanh Navy mang lại một sự mới mẻ.", 21, @"images\items\8\", converse, "Paepestraat", "178", "9260", "Wichelen", localMarket, Offer.No, true);
                Items Items09 = new Items("Converse Chuck Taylor", 1400000, "Toàn bộ thân giày được làm từ chất liệu da cao cấp cùng tone Xanh Navy mang lại một sự mới mẻ.", 47, @"images\items\9\", converse, "Arbeidstraat", "14", "9300", "Aalst", localMarket, Offer.No, true);
                Items Items10 = new Items("One Star Sunbaked", 1600000, "Kiểu dáng CONS One-Star với thiết kế All-White để bạn dễ dàng tận dụng với mọi outfit.", 22, @"images\items\10\", converse, "Ravensteinstraat", "50", "1000", "Brussel", localMarket, Offer.No, true);


                Items Items11 = new Items("Nike x CLOT Air Max Haven", 2200000, "Ideale nike voor oud en jong", 83, @"images\items\11\", nike, "Paepestraat", "178", "9260", "Wichelen", localMarket, Offer.No, true);
                Items Items12 = new Items("AIR JORDAN 1 RETRO HIGH OG", 2200000, "Bekenste nike van België met vestigingen over het hele land.", 75, @"images\items\12\", nike, "Arbeidstraat", "14", "9300", "Aalst", localMarket, Offer.No, true);
                Items Items13 = new Items("Air Jordan 11 Retro Low LE", 2200000, "Bekenste nike van België met vestigingen over het hele land.", 8, @"images\items\13\", nike, "Sint-Pietersnieuwstraat", "124", "9000", "Gent", localMarket, Offer.No, true);
                Items Items14 = new Items("Nike Epic Phantom React", 2200000, "Bekenste nike van België met vestigingen over het hele land.", 53, @"images\items\14\", nike, "Ravensteinstraat", "50", "1000", "Brussel", localMarket, Offer.No, true);
                Items Items15 = new Items("Nike Free RN Flyknit 3.0", 2200000, "Bekenste nike van België met vestigingen over het hele land.", 53, @"images\items\15\", nike, "Maalse Steenweg", "50", "8310", "Brugge", localMarket, Offer.No, true);
                Items Items16 = new Items("Nike Vaporfly 4% Flyknit", 2200000, "Bekenste nike van België met vestigingen over het hele land.", 72, @"images\items\16\", nike, "Luikersteenweg ", "40", "3800", "Sint-Truiden", localMarket, Offer.No, true);
                Items Items17 = new Items("NikeCourt Royale AC SE", 2200000, "Bekenste nike van België met vestigingen over het hele land.", 65, @"images\items\17\", nike, "Cooppallaan ", "40", "9230", "Wetteren", localMarket, Offer.No, true);
                Items Items18 = new Items("LeBron 16 SB", 2200000, "Bekenste nike van België met vestigingen over het hele land.", 22, @"images\items\18\", nike, "Paepestraat", "178", "9260", "Wichelen", localMarket, Offer.No, true);
                Items Items19 = new Items("Nike Free RN 5.0", 2200000, "Bekenste nike van België met vestigingen over het hele land.", 75, @"images\items\19\", nike, "Kasteeldreef", "15", "9340", "Lede", localMarket, Offer.No, true);


                Items Items20 = new Items("Adidas NMD_CS1 Primeknit", 2500000, "Geniet van de sterrenhemel in de mooie streken van Aalst (met gids).", 63, @"images\items\20\", adidas, "Arbeidstraat", "14", "9300", "Aalst", localMarket, Offer.No, true);
                Items Items21 = new Items("Adidas NMD R1", 2500000, "Wat is er nu leuker dan een dagje wallibi met vrienden", 34, @"images\items\21\", adidas, "Mechelsesteenweg ", "138", "9200", "Dendermonde", localMarket, Offer.No, true);
                Items Items22 = new Items("Adidas Stansmith", 2500000, "Spring zong er al over dus wat houd je tegen het te doen", 45, @"images\items\22\", adidas, "Torhoutsesteenweg", "611", "8400", "Oostende", localMarket, Offer.No, true);
                Items Items23 = new Items("Adidas UltraBoost 2.0", 2500000, "Disneyland de bestemming voor groot en klein", 35, @"images\items\23\", adidas, "Leopoldlaan", "1", "1930", "Zaventem", localMarket, Offer.No, true);
                Items Items24 = new Items("Adidas NMD_CS1 Primeknit", 2500000, "Het liedje zit ongetwijfeld al in je hoofd dus ga nu gewoon", 86, @"images\items\24\", adidas, "Rue Joseph Lamotte", "2", "5580", "Han-sur-Lesse", localMarket, Offer.No, true);
                Items Items25 = new Items("Adidas ZX 500 RM", 2500000, "Voor de oorlog fanaten een must", 35, @"images\items\25\", adidas, "Brandstraat", "57", "2830", "Willebroek", localMarket, Offer.No, true);


                Items Items26 = new Items("PAMPA OXFORD ORIGINALE", 1600000, "Stel zelf uw setje bloemen samen met deze items.", 43, @"images\items\26\", palladium, "Arbeidstraat", "14", "9300", "Aalst", localMarket, Offer.No, true);
                Items Items27 = new Items("PAMPALICIOUS POP CORN", 1600000, "Heb je grond nodig voor in een pot", 68, @"images\items\27\", palladium, "Kasteeldreef", "15", "9340", "Lede", localMarket, Offer.No, true);
                Items Items28 = new Items("PAMPALICIOUS STARLIGHT BLUE", 1600000, "Schelfhout, waar moet je andes zijn!", 75, @"images\items\28\", palladium, "Arbeidstraat", "14", "9300", "Aalst", localMarket, Offer.No, true);
                Items Items29 = new Items("PAMPALICIOUS BLOSSOM", 1600000, "Lies, verkoopt ook wel een madelief ", 25, @"images\items\29\", palladium, "Sint-Pietersnieuwstraat", "124", "9000", "Gent", localMarket, Offer.No, true);
                Items Items30 = new Items("PALLADIUM PAMPA HI DARE", 1600000, "Vissen, fonteinen, dieraccesoire...", 14, @"images\items\30\", palladium, "Kasteeldreef", "15", "9340", "Lede", localMarket, Offer.No, true);
                Items Items31 = new Items("PALLADIUM PAMPA HI DARE", 1600000, "Blub, de winkel voor vis enthousiasten", 35, @"images\items\31\", palladium, "Arbeidstraat", "14", "9300", "Aalst", localMarket, Offer.No, true);
                Items Items32 = new Items("PALLADIUM PAMPA FREE CANVAS", 1600000, "Jaren ervaring in het snoeien", 76, @"images\items\32\", palladium, "Paepestraat", "178", "9260", "Wichelen", localMarket, Offer.No, true);
                Items Items33 = new Items("PAMPA LITE CUFF WATERPROOF", 1600000, "Bij de boerenItemsd vind je altijd wat je zoekt", 75, @"images\items\33\", palladium, "Arbeidstraat", "14", "9300", "Aalst", localMarket, Offer.No, true);
                Items Items34 = new Items("PALLABROUSE BAGGY", 1600000, "Gazon voorzieningen", 14, @"images\items\34\", palladium, "Cooppallaan ", "40", "9230", "Wetteren", localMarket, Offer.No, true);

                Items Items35 = new Items("VANS ERA GET THE REAL", 1700000, "Pukkelpop, dat moet je gedaan hebben", 57, @"images\items\35\", vans, "Paepestraat", "178", "9260", "Wichelen", localMarket, Offer.No, true);
                Items Items36 = new Items("VANS UA OLD SKOOL I HEART ", 1700000, "Bierfanaten kunnen dit niet missen", 75, @"images\items\36\", vans, "Kleine Dam", "1", "9160", "Lokeren", localMarket, Offer.No, true);
                Items Items37 = new Items("VANS OLD SKOOL OT SIDEWALL", 1700000, "Drinken en eten, meer moet dat niet zijn", 25, @"images\items\37\", vans, "Sint-Pietersnieuwstraat", "124", "9000", "Gent", localMarket, Offer.No, true);
                Items Items38 = new Items("ANAHEIM FACTORY STYLE 73 DX", 1700000, "Voor elke nerd wat wils", 14, @"images\items\38\", vans, "Ravensteinstraat", "50", "1000", "Brussel", localMarket, Offer.No, true);
                Items Items39 = new Items("CLASSIC COM AUTHENTIC", 1700000, "Ideal geshenk voor een autofanaat", 38, @"images\items\39\", vans, "Paepestraat", "178", "9260", "Wichelen", localMarket, Offer.No, true);
                Items Items40 = new Items("COMFYCUSH OLD SKOOL", 1700000, "Cosplay, eten en vervoer", 18, @"images\items\40\", vans, "Sint-Pietersnieuwstraat", "124", "9000", "Gent", localMarket, Offer.No, true);


                _dbContext.Items.AddRange(Items01, Items02, Items03, Items04, Items05, Items06, Items07, Items08, Items09, Items10, Items11, Items12, Items13, Items14, Items15, Items16, Items17, Items18, Items19, Items20, Items21, Items22, Items23, Items24, Items25, Items26, Items27, Items28, Items29, Items30, Items31, Items32, Items33, Items34, Items35, Items36, Items37, Items38, Items39, Items40);

                _dbContext.SaveChanges();
            }
        }

        private async Task CreateUser(string userName, string email, string password, string role)
        {
            var user = new ApplicationUser { UserName = userName, Email = email, EmailConfirmed = true };
            await _userManager.CreateAsync(user, password);
            await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, role));
        }
    }
}
