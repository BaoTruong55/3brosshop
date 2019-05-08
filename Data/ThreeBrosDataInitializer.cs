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
//                Category beauty = new Category("Beauty", "fa-female");
//                Category interieur = new Category("Interieur", "fa-image");
//                Category kledij = new Category("Kledij", "fa-umbrella");
//                Category multimedia = new Category("Multimedia", "fa-laptop");
//                Category generiek = new Category("Generiek", "fa-gift");

                var categories = new List<Category>
                {
                    converse, vans, nike, adidas, palladium
                };
                _dbContext.Category.AddRange(categories);


                Seller Handelaar01 = new Seller("1970s Base Camp Suede", "lekker@gmail.com", "Vải lưới ở lưỡi gà và dây giày giúp sản phẩm trở nên nổi bật hơn",  "Arbeidstraat", "14", "9300", "Aalst", true);
                Seller Handelaar02 = new Seller("1970s Vintage Canvas", "bontinck@gmail.com", "Vẫn là màu vàng best-seller của nhà Converse.",  "Paepestraat", "178", "9260", "Wichelen", true);
                Seller Handelaar03 = new Seller("1970s Vintage Canvas", "schets@gmail.com", "Tone màu xanh lạ mắt, dễ chịu nhưng không kém phần cá tính.",  "Ravensteinstraat", "50", "1000", "Brussel", true);
                Seller Handelaar04 = new Seller("Chuck Taylor All Star 1970s", "coninck@gmail.com", "Thiết kế cổ điển của Chuck 1970s với tone đen huyền thoại ",  "Sint-Pietersnieuwstraat", "124", "9000", "Gent", true);
                Seller Handelaar05 = new Seller("Chuck Taylor All Star 1970s", "Handelaar01@gmail.com", "Phiên bản cổ thấp màu đen cá tính của Chuck 1970s được giới trẻ nhiệt tình ưu ái",  "Arbeidstraat", "14", "9300", "Aalst", true);
                Seller Handelaar06 = new Seller("1970s Base Camp Suede", "sandwich@gmail.com", "Vải lưới ở lưỡi gà và dây giày giúp sản phẩm trở nên nổi bật hơn.",  "Arbeidstraat", "14", "9300", "Aalst", true);
                Seller Handelaar07 = new Seller("McDonalds", "mc@gmail.com", "Voor een snelle hap moet u bij ons zijn!",  "Arbeidstraat", "14", "9300", "Aalst", true);
                Seller Handelaar08 = new Seller("SOS Piet", "sospiet@gmail.com", "Het echte restaurant van SOS Piet. Altijd de beste maatlijd voor een gezonde prijs!",  "Paepestraat", "178", "9260", "Wichelen", true);
                Seller Handelaar09 = new Seller("CoBoSh", "cobosh@gmail.com", "Voor de beste wijnen moet je bij ons zijn! Hierbij kan altijd een hapje geserveerd worden.",  "Arbeidstraat", "14", "9300", "Aalst", true);

                Seller Handelaar11 = new Seller("Sanitas", "sanitas@gmail.com", "Bekenste nike van Wichelen.",  "Paepestraat", "178", "9260", "Wichelen", true);
                Seller Handelaar12 = new Seller("nike Basic-Fit", "basicfit@gmail.com", "Bekenste nike van België met vestigingen over het hele land.",  "Ravensteinstraat", "50", "1000", "Brussel", true);

                Seller Handelaar21 = new Seller("Aalst", "aalst@gmail.com", "De recreatiedienst van Aalst staat in voor tal van speciale activiteiten.",  "Arbeidstraat", "14", "9300", "Aalst", true);
                Seller Handelaar22 = new Seller("Walibi", "walibi@gmail.com", "Een pretpark voor klein en groot.",  "Mechelsesteenweg ", "138", "9200", "Dendermonde", true);
                Seller Handelaar23 = new Seller("NMBS", "trein@gmail.com", "De spoorwegdienst van België. Staakt liever dan te werken.",  "Torhoutsesteenweg", "611", "8400", "Oostende", true);
                Seller Handelaar24 = new Seller("Disneyland Paris", "parijs@gmail.com", "Een van de grootste pretparken in Frankrijk.",  "Leopoldlaan", "1", "1930", "Zaventem", true);
                Seller Handelaar25 = new Seller("Hamme", "hamme@gmail.com", "Stad Hamme",  "Rue Joseph Lamotte", "2", "5580", "Han-sur-Lesse", true);
                Seller Handelaar26 = new Seller("Breemdonk", "breemdonk@gmail.com", "City Breemdonk",  "Brandstraat", "57", "2830", "Willebroek", true);

                Seller Handelaar31 = new Seller("Brenk", "brenk@gmail.com", "Stel zelf uw setje bloemen samen met deze items.",  "Arbeidstraat", "14", "9300", "Aalst", true);
                Seller Handelaar32 = new Seller("De Mol", "molleken@gmail.com", "Heb je grond nodig voor in een pot",  "Kasteeldreef", "15", "9340", "Lede", true);
                Seller Handelaar33 = new Seller("Schelfhout", "schelfhout@gmail.com", "Schelfhout, waar moet je andes zijn!",  "Arbeidstraat", "14", "9300", "Aalst", true);
                Seller Handelaar34 = new Seller("Liesje", "lies@gmail.com", "Lies, verkoopt ook wel een madelief",  "Sint-Pietersnieuwstraat", "124", "9000", "Gent", true);
                Seller Handelaar35 = new Seller("Funa Lima", "funa_lima@gmail.com", "Vissen, fonteinen, dieraccesoire...",  "Kasteeldreef", "15", "9340", "Lede", true);
                Seller Handelaar36 = new Seller("Blub", "blub@gmail.com", "Blub, de winkel voor vis enthousiasten",  "Arbeidstraat", "14", "9300", "Aalst", true);
                Seller Handelaar37 = new Seller("G-Bont", "grasb@gmail.com", "Jaren ervaring in het snoeien van alle gazons",  "Paepestraat", "178", "9260", "Wichelen", true);
                Seller Handelaar38 = new Seller("Aveve", "aveve@gmail.com", "Bij de boerenbond vind je altijd wat je zoekt",  "Arbeidstraat", "14", "9300", "Aalst", true);
                Seller Handelaar39 = new Seller("Groener Gras", "groengras@gmail.com", "Gazon voorzieningen voor iedereen die een groen gazon wil!",  "Cooppallaan ", "40", "9230", "Wetteren", true);

                Seller Handelaar41 = new Seller("Pukkelpop", "ppk@gmail.com", "Tickets of coupons voor pukkelpok.",  "Paepestraat", "178", "9260", "Wichelen", true);
                Seller Handelaar42 = new Seller("Bierfeesten", "bierfeesten@gmail.com", "De veste feesten in Lokeren: De Lokerse Bierfeesten!",  "Kleine Dam", "1", "9160", "Lokeren", true);
                Seller Handelaar43 = new Seller("Gentse Feesten", "feesten-gent@gmail.com", "Het grootste feest in Gent!",  "Sint-Pietersnieuwstraat", "124", "9000", "Gent", true);
                Seller Handelaar44 = new Seller("Gameforce", "games@gmail.com", "Grootste game beurs in België. Nu ook kortingsbonnen verkrijgbaar!",  "Ravensteinstraat", "50", "1000", "Brussel", true);
                Seller Handelaar45 = new Seller("Garage Ferrari", "ferfer@gmail.com", "Beste cadeau voor een Ferrari liefhebber!",  "Paepestraat", "178", "9260", "Wichelen", true);
                Seller Handelaar46 = new Seller("Facts", "facts@gmail.com", "Een van de grootste cosplay beurzen van België.",  "Sint-Pietersnieuwstraat", "124", "9000", "Gent", true);

                Seller Handelaar51 = new Seller("Nude", "nude@gmail.com", "Het bekendste merk voor beauty producten!",  "Paepestraat", "178", "9260", "Wichelen", true);
                Seller Handelaar52 = new Seller("Restaurant Lekker", "lekker@gmail.com", "korte beschrijving",  "Arbeidstraat", "14", "9300", "Aalst", true);
                Seller Handelaar53 = new Seller("Ici Paris", "ici-paris@gmail.com", "Voor een parfum moet je bij ons zijn!",  "Arbeidstraat", "14", "9300", "Aalst", true);

                Seller Handelaar61 = new Seller("Sofa & Co", "sofaco@gmail.com", "Vind de gepaste sofa bij ons!",  "Arbeidstraat", "14", "9300", "Aalst", true);
                Seller Handelaar62 = new Seller("Deba", "deba@gmail.com", "Voor elk interieur stuk kan u bij ons terecht!",  "Arbeidstraat", "14", "9300", "Aalst", true);
                Seller Handelaar63 = new Seller("Ikea", "ikea@gmail.com", "Hebt u iets nodig tegen een lage prijs dan kan u altijd bij ons terecht.",  "Paepestraat", "178", "9260", "Wichelen", true);
                Seller Handelaar64 = new Seller("Leen Bakker", "leen-bakker@gmail.com", "Iets kopen dan bent u op de juiste plaats.",  "Sint-Pietersnieuwstraat", "124", "9000", "Gent", true);
                Seller Handelaar65 = new Seller("Salon Ballon", "salon-ballon@gmail.com", "De speciaal zaak die u zocht.",  "Sint-Pietersnieuwstraat", "124", "9000", "Gent", true);
                Seller Handelaar66 = new Seller("Donald", "donald-keukens@gmail.com", "Keuken nodig kom dan bij ons!",  "Arbeidstraat", "14", "9300", "Aalst", true);
                Seller Handelaar67 = new Seller("Modern Gent", "gent-modern@gmail.com", "Modern interieur tegen een prijsje.",  "Sint-Pietersnieuwstraat", "124", "9000", "Gent", true);
                Seller Handelaar68 = new Seller("Kunst & Kitch", "kunst-kitch@gmail.com", "Kunst hoeft niet altijd lelijk te zijn.",  "Paepestraat", "178", "9260", "Wichelen", true);
                Seller Handelaar69 = new Seller("Gill", "gill@gmail.com", "Interieur tegen een prijsje.",  "Arbeidstraat", "14", "9300", "Aalst", true);

                Seller Handelaar71 = new Seller("C&A", "cena@gmail.com", "De Kleding winkel van Aalst.",  "Arbeidstraat", "14", "9300", "Aalst", true);
                Seller Handelaar72 = new Seller("AS Adventure", "as-adventure@gmail.com", "Outdoor kleding en alles voor je outdoor ervaring.",  "Paepestraat", "178", "9260", "Wichelen", true);
                Seller Handelaar73 = new Seller("Ultra Wet", "ultra-wet@gmail.com", "De kldeingspecialist voor droog en nat.",  "Arbeidstraat", "14", "9300", "Aalst", true);
                Seller Handelaar74 = new Seller("Holiday", "holiday@gmail.com", "Voor al uw feestkledij.",  "Sint-Pietersnieuwstraat", "124", "9000", "Gent", true);
                Seller Handelaar75 = new Seller("Bram's Fashion", "bram@gmail.com", "Voor ieder wat wilds.",  "Paepestraat", "178", "9260", "Wichelen", true);
                Seller Handelaar76 = new Seller("Bontinck Panther's", "panther@gmail.com", "Pants from Bontinck are dreams for legs.",  "Arbeidstraat", "14", "9300", "Aalst", true);
                Seller Handelaar77 = new Seller("Bre Bra", "bre-bra@gmail.com", "Van A tot Z  u vindt het bij ons.",  "Sint-Pietersnieuwstraat", "124", "9000", "Gent", true);
                Seller Handelaar78 = new Seller("Pikantje", "pikant@gmail.com", "Eroiek u vindt het bij ons.",  "Arbeidstraat", "14", "9300", "Aalst", true);

                Seller Handelaar81 = new Seller("Fnac", "fnac@gmail.com", "De multimedia specialist in Europa.",  "Arbeidstraat", "14", "9300", "Aalst", true);
                Seller Handelaar82 = new Seller("Mediamarkt", "mediamarkt@gmail.com", "Electronica tegen een spot prijs.",  "Mechelsesteenweg", "138", "9200", "Dendermonde", true);
                Seller Handelaar83 = new Seller("Van Den Borre", "vandenborre@gmail.com", "Koffiezets voor 12€.",  "Sint-Pietersnieuwstraat", "124", "9000", "Gent", true);
                Seller Handelaar84 = new Seller("Bontinck IT", "bontinck-it@gmail.com", "Een probleempje groot of klein, dan moet je bij IT Lennert zijn.",  "Maalse Steenweg", "50", "8310", "Brugge", true);
                Seller Handelaar85 = new Seller("Schets Apple Premium", "schets-apple@gmail.com", "Alle laatste Apple producten moet u bij ons zijn.",  "Paepestraat", "178", "9260", "Wichelen", true);
                Seller Handelaar86 = new Seller("Lab9", "lab-9@gmail.com", "Officiele Apple reseller.",  "Arbeidstraat", "14", "9300", "Aalst", true);
                Seller Handelaar87 = new Seller("De Conincks Screen Repair", "screenrepair@gmail.com", "Een ongelukje is snel gebeurd.",  "Paepestraat", "178", "9260", "Wichelen", true);
                Seller Handelaar88 = new Seller("Medion Custom", "medion@gmail.com", "Medion laptop op maat gemaakt",  "Sint-Pietersnieuwstraat", "124", "9000", "Gent", true);
                Seller Handelaar89 = new Seller("Dell Dinosaur", "dell@gmail.com", "MS Dos specialist",  "Paepestraat", "178", "9260", "Wichelen", true);


                Seller Handelaar91 = new Seller("Generiek", "generiek@gmail.com", "generiek",  "Ravensteinstraat", "50", "1000", "Brussel", true);

                Seller Handelaar10 = new Seller("ChaCha", "chacha@gmail.com", "Voor de beste wijnen moet je bij ons zijn! Hierbij kan altijd een hapje geserveerd worden.",  "Ravensteinstraat", "50", "1000", "Brussel", true);
                var handelaars = new List<Seller>
                {
                    Handelaar01, Handelaar02, Handelaar03, Handelaar04, Handelaar05, Handelaar06, Handelaar07, Handelaar08, Handelaar09, Handelaar10, Handelaar11, Handelaar21, Handelaar22, Handelaar23, Handelaar24, Handelaar25, Handelaar26, Handelaar31, Handelaar32, Handelaar33, Handelaar34, Handelaar35, Handelaar36, Handelaar37, Handelaar38, Handelaar39, Handelaar41, Handelaar42, Handelaar43, Handelaar44, Handelaar45, Handelaar46, Handelaar51, Handelaar52, Handelaar53, Handelaar61, Handelaar62, Handelaar63, Handelaar64, Handelaar65, Handelaar66, Handelaar67, Handelaar68, Handelaar69, Handelaar71, Handelaar72, Handelaar73, Handelaar74, Handelaar75, Handelaar76, Handelaar77, Handelaar78, Handelaar81, Handelaar82, Handelaar83, Handelaar84, Handelaar85, Handelaar86, Handelaar87, Handelaar88, Handelaar89, Handelaar91
                };

                _dbContext.Seller.AddRange(handelaars);

                Items bon01 = new Items("1970s Base Camp Suede", 1700000, "Vải lưới ở lưỡi gà và dây giày giúp sản phẩm trở nên nổi bật hơn", 17, @"images\items\1\", converse, "Sư Vạn Hạnh", "14", "9300", "Hồ Chí Minh", Handelaar01, Offer.No, true);
                Items bon02 = new Items("1970s Vintage Canvas", 1700000, "Vẫn là màu vàng best-seller của nhà Converse.", 242, @"images\items\2\", converse, "Paepestraat", "178", "9260", "Wichelen", Handelaar02, Offer.Slider, true);
                Items bon03 = new Items("1970s Vintage Canvas", 1700000, "Tone màu xanh lạ mắt, dễ chịu nhưng không kém phần cá tính.", 42, @"images\items\3\", converse, "Ravensteinstraat", "50", "1000", "Brussel", Handelaar03, Offer.No, true);
                Items bon04 = new Items("Chuck Taylor All Star 1970s", 1800000, "Thiết kế cổ điển của Chuck 1970s với tone đen huyền thoại .", 24, @"images\items\4\", converse, "Sint-Pietersnieuwstraat", "124", "9000", "Gent", Handelaar04, Offer.No, true);
                Items bon05 = new Items("Chuck Taylor All Star 1970s", 1400000, "Phiên bản cổ thấp màu đen cá tính của Chuck 1970s được giới trẻ nhiệt tình ưu ái.", 124, @"images\items\5\", converse, "Arbeidstraat", "14", "9300", "Aalst", Handelaar05, Offer.Slider, true);
                Items bon06 = new Items("1970s Base Camp Suede", 1500000, "Vải lưới ở lưỡi gà và dây giày giúp sản phẩm trở nên nổi bật hơn.", 45, @"images\items\6\", converse, "Arbeidstraat", "14", "9300", "Aalst", Handelaar06, Offer.No, true);
                Items bon07 = new Items("Chuck Taylor All Star", 1300000,  "Với 2 tone màu Xanh Navy và Xanh Chàm tươi mát cùng xuất hiện ngay trên đôi giày sẽ giúp bạn có được một sản phẩm không bao giờ lỗi mốt.", 98, @"images\items\7\", converse, "Arbeidstraat", "14", "9300", "Aalst", Handelaar07, Offer.No, true);
                Items bon08 = new Items("All Star Limo Leather", 1700000,  "Toàn bộ thân giày được làm từ chất liệu da cao cấp cùng tone Xanh Navy mang lại một sự mới mẻ.", 21, @"images\items\8\", converse, "Paepestraat", "178", "9260", "Wichelen", Handelaar08, Offer.No, true);
                Items bon09 = new Items("Converse Chuck Taylor", 1400000,  "Toàn bộ thân giày được làm từ chất liệu da cao cấp cùng tone Xanh Navy mang lại một sự mới mẻ.", 47, @"images\items\9\", converse, "Arbeidstraat", "14", "9300", "Aalst", Handelaar09, Offer.No, true);
                Items bon10 = new Items("One Star Sunbaked", 1600000,  "Kiểu dáng CONS One-Star với thiết kế All-White để bạn dễ dàng tận dụng với mọi outfit.", 22, @"images\items\10\", converse, "Ravensteinstraat", "50", "1000", "Brussel", Handelaar10, Offer.No, true);


                Items bon11 = new Items("Nike x CLOT Air Max Haven", 2200000,  "Ideale nike voor oud en jong", 83, @"images\items\11\", nike, "Paepestraat", "178", "9260", "Wichelen", Handelaar11, Offer.No, true);
                Items bon12 = new Items("AIR JORDAN 1 RETRO HIGH OG", 2200000,  "Bekenste nike van België met vestigingen over het hele land.", 75, @"images\items\12\", nike, "Arbeidstraat", "14", "9300", "Aalst", Handelaar12, Offer.No, true);
                Items bon13 = new Items("Air Jordan 11 Retro Low LE", 2200000,  "Bekenste nike van België met vestigingen over het hele land.", 8, @"images\items\13\", nike, "Sint-Pietersnieuwstraat", "124", "9000", "Gent", Handelaar12, Offer.No, true);
                Items bon14 = new Items("Nike Epic Phantom React", 2200000, "Bekenste nike van België met vestigingen over het hele land.", 53, @"images\items\14\", nike, "Ravensteinstraat", "50", "1000", "Brussel", Handelaar12, Offer.No, true);
                Items bon15 = new Items("Nike Free RN Flyknit 3.0", 2200000, "Bekenste nike van België met vestigingen over het hele land.", 53, @"images\items\15\", nike, "Maalse Steenweg", "50", "8310", "Brugge", Handelaar12, Offer.No, true);
                Items bon16 = new Items("Nike Vaporfly 4% Flyknit", 2200000, "Bekenste nike van België met vestigingen over het hele land.", 72, @"images\items\16\", nike, "Luikersteenweg ", "40", "3800", "Sint-Truiden", Handelaar12, Offer.No, true);
                Items bon17 = new Items("NikeCourt Royale AC SE", 2200000, "Bekenste nike van België met vestigingen over het hele land.", 65, @"images\items\17\", nike, "Cooppallaan ", "40", "9230", "Wetteren", Handelaar12, Offer.No, true);
                Items bon18 = new Items("LeBron 16 SB", 2200000,  "Bekenste nike van België met vestigingen over het hele land.", 22, @"images\items\18\", nike, "Paepestraat", "178", "9260", "Wichelen", Handelaar12, Offer.No, true);
                Items bon19 = new Items("Nike Free RN 5.0", 2200000, "Bekenste nike van België met vestigingen over het hele land.", 75, @"images\items\19\", nike, "Kasteeldreef", "15", "9340", "Lede", Handelaar12, Offer.No, true);


                Items bon20 = new Items("Adidas NMD_CS1 Primeknit", 2500000, "Geniet van de sterrenhemel in de mooie streken van Aalst (met gids).", 63, @"images\items\20\", adidas, "Arbeidstraat", "14", "9300", "Aalst", Handelaar21, Offer.No, true);
                Items bon21 = new Items("Adidas NMD R1", 2500000,  "Wat is er nu leuker dan een dagje wallibi met vrienden", 34, @"images\items\21\", adidas, "Mechelsesteenweg ", "138", "9200", "Dendermonde", Handelaar22, Offer.No, true);
                Items bon22 = new Items("Adidas Stansmith", 2500000,  "Spring zong er al over dus wat houd je tegen het te doen", 45, @"images\items\22\", adidas, "Torhoutsesteenweg", "611", "8400", "Oostende", Handelaar23, Offer.No, true);
                Items bon23 = new Items("Adidas UltraBoost 2.0", 2500000,  "Disneyland de bestemming voor groot en klein", 35, @"images\items\23\", adidas, "Leopoldlaan", "1", "1930", "Zaventem", Handelaar24, Offer.No, true);
                Items bon24 = new Items("Adidas NMD_CS1 Primeknit", 2500000,  "Het liedje zit ongetwijfeld al in je hoofd dus ga nu gewoon", 86, @"images\items\24\", adidas, "Rue Joseph Lamotte", "2", "5580", "Han-sur-Lesse", Handelaar25, Offer.No, true);
                Items bon25 = new Items("Adidas ZX 500 RM", 2500000, "Voor de oorlog fanaten een must", 35, @"images\items\25\", adidas, "Brandstraat", "57", "2830", "Willebroek", Handelaar26, Offer.No, true);


                Items bon26 = new Items("PAMPA OXFORD ORIGINALE", 1600000,  "Stel zelf uw setje bloemen samen met deze items.", 43, @"images\items\26\", palladium, "Arbeidstraat", "14", "9300", "Aalst", Handelaar31, Offer.No, true);
                Items bon27 = new Items("PAMPALICIOUS POP CORN", 1600000, "Heb je grond nodig voor in een pot", 68, @"images\items\27\", palladium, "Kasteeldreef", "15", "9340", "Lede", Handelaar32, Offer.No, true);
                Items bon28 = new Items("PAMPALICIOUS STARLIGHT BLUE", 1600000,  "Schelfhout, waar moet je andes zijn!", 75, @"images\items\28\", palladium, "Arbeidstraat", "14", "9300", "Aalst", Handelaar33, Offer.No, true);
                Items bon29 = new Items("PAMPALICIOUS BLOSSOM", 1600000,  "Lies, verkoopt ook wel een madelief ", 25, @"images\items\29\", palladium, "Sint-Pietersnieuwstraat", "124", "9000", "Gent", Handelaar34, Offer.No, true);
                Items bon30 = new Items("PALLADIUM PAMPA HI DARE", 1600000,  "Vissen, fonteinen, dieraccesoire...", 14, @"images\items\30\", palladium, "Kasteeldreef", "15", "9340", "Lede", Handelaar35, Offer.No, true);
                Items bon31 = new Items("PALLADIUM PAMPA HI DARE", 1600000,  "Blub, de winkel voor vis enthousiasten", 35, @"images\items\31\", palladium, "Arbeidstraat", "14", "9300", "Aalst", Handelaar36, Offer.No, true);
                Items bon32 = new Items("PALLADIUM PAMPA FREE CANVAS", 1600000, "Jaren ervaring in het snoeien", 76, @"images\items\32\", palladium, "Paepestraat", "178", "9260", "Wichelen", Handelaar37, Offer.No, true);
                Items bon33 = new Items("PAMPA LITE CUFF WATERPROOF", 1600000, "Bij de boerenbond vind je altijd wat je zoekt", 75, @"images\items\33\", palladium, "Arbeidstraat", "14", "9300", "Aalst", Handelaar38, Offer.No, true);
                Items bon34 = new Items("PALLABROUSE BAGGY", 1600000, "Gazon voorzieningen", 14, @"images\items\34\", palladium, "Cooppallaan ", "40", "9230", "Wetteren", Handelaar39, Offer.No, true);

                Items bon35 = new Items("VANS ERA GET THE REAL", 1700000,  "Pukkelpop, dat moet je gedaan hebben", 57, @"images\items\35\", vans, "Paepestraat", "178", "9260", "Wichelen", Handelaar41, Offer.No, true);
                Items bon36 = new Items("VANS UA OLD SKOOL I HEART ", 1700000, "Bierfanaten kunnen dit niet missen", 75, @"images\items\36\", vans, "Kleine Dam", "1", "9160", "Lokeren", Handelaar42, Offer.No, true);
                Items bon37 = new Items("VANS OLD SKOOL OT SIDEWALL", 1700000,  "Drinken en eten, meer moet dat niet zijn", 25, @"images\items\37\", vans, "Sint-Pietersnieuwstraat", "124", "9000", "Gent", Handelaar43, Offer.No, true);
                Items bon38 = new Items("ANAHEIM FACTORY STYLE 73 DX", 1700000, "Voor elke nerd wat wils", 14, @"images\items\38\", vans, "Ravensteinstraat", "50", "1000", "Brussel", Handelaar44, Offer.No, true);
                Items bon39 = new Items("CLASSIC COM AUTHENTIC", 1700000,  "Ideal geshenk voor een autofanaat", 38, @"images\items\39\", vans, "Paepestraat", "178", "9260", "Wichelen", Handelaar45, Offer.No, true);
                Items bon40 = new Items("COMFYCUSH OLD SKOOL", 1700000,  "Cosplay, eten en vervoer", 18, @"images\items\40\", vans, "Sint-Pietersnieuwstraat", "124", "9000", "Gent", Handelaar46, Offer.No, true);

//                Items bon41 = new Items("Makeup pallete Nude", 29, "Het bekendste merk zijn palette", 67, @"images\items\41\", beauty, "Paepestraat", "178", "9260", "Wichelen", Handelaar51, Offer.No, true);
//                Items bon42 = new Items("Ici Paris verwenbon", 15, "Een parfum kan je nooit mee misdoen", 17, @"images\items\42\", beauty, "Arbeidstraat", "14", "9300", "Aalst", Handelaar52, Offer.No, true);
//                Items bon43 = new Items("Lipstick Lover Aalst", 8,  "Voor de lippen lovers", 86, @"images\items\43\", beauty, "Arbeidstraat", "14", "9300", "Aalst", Handelaar53, Offer.No, true);
//
//                Items bon44 = new Items("Sofa en Co", 18, "Relax in een sofa van sofa en co", 71, @"images\items\44\", interieur, "Arbeidstraat", "14", "9300", "Aalst", Handelaar61, Offer.No, true);
//                Items bon45 = new Items("Deba meubelen", 36,  "Verkoopt al uw interieur", 37, @"images\items\45\", interieur, "Arbeidstraat", "14", "9300", "Aalst", Handelaar62, Offer.No, true);
//                Items bon46 = new Items("Ikea huisvoorzieningen", 40,  "Meubelspiaclist sinds 74", 71, @"images\items\46\", interieur, "Paepestraat", "178", "9260", "Wichelen", Handelaar63, Offer.No, true);
//                Items bon47 = new Items("Leenbakker", 34,  "Om te kopen, niet te lenen", 37, @"images\items\47\", interieur, "Sint-Pietersnieuwstraat", "124", "9000", "Gent", Handelaar64, Offer.No, true);
//                Items bon48 = new Items("Salon Ballon Gent", 13,  "Sallon Ballon is een speciaalzaak te Gent", 76, @"images\items\48\", interieur, "Sint-Pietersnieuwstraat", "124", "9000", "Gent", Handelaar65, Offer.No, true);
//                Items bon49 = new Items("Keukens Donald", 45, "Al 8 jaar maak ik keukens alsof ze voor mezelf zijn", 46, @"images\items\49\", interieur, "Arbeidstraat", "14", "9300", "Aalst", Handelaar66, Offer.No, true);
//                Items bon50 = new Items("Modern Gent", 31,  "Modern interieur hoeft niet duur te zijn", 75, @"images\items\50\", interieur, "Sint-Pietersnieuwstraat", "124", "9000", "Gent", Handelaar67, Offer.No, true);
//                Items bon51 = new Items("Kunst & Kitch", 18,  "Een beetje kunst, een beetje kitch", 46, @"images\items\51\", interieur, "Paepestraat", "178", "9260", "Wichelen", Handelaar68, Offer.No, true);
//                Items bon52 = new Items("Moderne interieur Gill", 5,  "Op maat gemaakt interieur tegen een zacht prijsje", 45, @"images\items\52\", interieur, "Arbeidstraat", "14", "9300", "Aalst", Handelaar69, Offer.No, true);
//
//                Items bon53 = new Items("C&A Aalst", 24,  "De kledingwinkel in Aalst en omstreken", 42, @"images\items\53\", kledij, "Arbeidstraat", "14", "9300", "Aalst", Handelaar71, Offer.No, true);
//                Items bon54 = new Items("AS Advantures", 48,  "Kledie om een avontuur mee aan te gaan", 47, @"images\items\54\", kledij, "Paepestraat", "178", "9260", "Wichelen", Handelaar72, Offer.No, true);
//                Items bon55 = new Items("Ultra Wet", 41,  "De kledingspecialist voor droog en nat", 71, @"images\items\55\", kledij, "Arbeidstraat", "14", "9300", "Aalst", Handelaar73, Offer.No, true);
//                Items bon56 = new Items("Holiday", 24,  "Voor al uw feestkledij", 17, @"images\items\56\", kledij, "Sint-Pietersnieuwstraat", "124", "9000", "Gent", Handelaar74, Offer.No, true);
//                Items bon57 = new Items("Bram's Fashion", 23, "Voor ieder wat wilds", 73, @"images\items\57\", kledij, "Paepestraat", "178", "9260", "Wichelen", Handelaar75, Offer.Standard, true);
//                Items bon58 = new Items("Bontinck's Panthers", 30,  "Pants from Bontinck are dreams for legs", 72, @"images\items\58\", kledij, "Arbeidstraat", "14", "9300", "Aalst", Handelaar76, Offer.No, true);
//                Items bon59 = new Items("Bre Bra", 29,  "Van A tot Z, u vindt het bij ons", 92, @"images\items\59\", kledij, "Sint-Pietersnieuwstraat", "124", "9000", "Gent", Handelaar77, Offer.Standard, true);
//                Items bon69 = new Items("Pikantje", 34,  "Erotische kledingwinkel", 9, @"images\items\69\", kledij, "Arbeidstraat", "14", "9300", "Aalst", Handelaar78 , Offer.No, true);
//
//                Items bon61 = new Items("Fnac Aalst", 3,  "De multimedia specialist in Europa", 54, @"images\items\61\", multimedia, "Arbeidstraat", "14", "9300", "Aalst", Handelaar81, Offer.No, true);
//                Items bon62 = new Items("Mediamarkt Dendermonde", 46,  "Electronica tegen een spot prijs", 45, @"images\items\62\", multimedia, "Mechelsesteenweg", "138", "9200", "Dendermonde", Handelaar82, Offer.No, true);
//                Items bon63 = new Items("Van Den Borre Gent", 31,  "Koffiezets voor 12€", 15, @"images\items\63\", multimedia, "Sint-Pietersnieuwstraat", "124", "9000", "Gent", Handelaar83, Offer.No, true);
//                Items bon64 = new Items("Bontinck IT brugge", 36,  "Een probleempje groot of klein, dan moet je bij IT Lennert zijn", 67, @"images\items\64\", multimedia, "Maalse Steenweg", "50", "8310", "Brugge", Handelaar84, Offer.Standard, true);
//                Items bon65 = new Items("Schets Apple Premium", 49,  "U vindt alle laatste Apple producten hier", 78, @"images\items\65\", multimedia, "Paepestraat", "178", "9260", "Wichelen", Handelaar85, Offer.No, true);
//                Items bon66 = new Items("Lab9 Aalst", 7,  "Officiele Apple reseller", 64, @"images\items\66\", multimedia, "Arbeidstraat", "14", "9300", "Aalst", Handelaar86, Offer.No, true);
//                Items bon67 = new Items("De Conincks Screen Repair", 38,  "Een ongelukje is rap gebeurd", 75, @"images\items\67\", multimedia, "Paepestraat", "178", "9260", "Wichelen", Handelaar87, Offer.No, true);
//                Items bon68 = new Items("Medion Custom", 40,  "Medion laptop op maat gemaakt", 24, @"images\items\68\", multimedia, "Sint-Pietersnieuwstraat", "124", "9000", "Gent", Handelaar88, Offer.No, true);
//                Items bon60 = new Items("Dell Dinosaur", 27, "MS Dos specialist", 30, @"images\items\60\", multimedia, "Paepestraat", "178", "9260", "Wichelen", Handelaar89, Offer.No, true);
//
//
//                Items bon70 = new Items("Generieke cadeaubon", 1, "Niet zeker welke items u juist wilt, dan is deze generieke items iets voor u!", 457, @"images\items\70\", generiek, "Arbeidstraat", "14", "9300", "Aalst", Handelaar91, Offer.Slider, true);
                var bonnen = new List<Items>
                {
                   bon01, bon02, bon03, bon04, bon05, bon06, bon07, bon08, bon09, bon10, bon11, bon12, bon13, bon14, bon15, bon16, bon17, bon18, bon19, bon20, bon21, bon22, bon23, bon24, bon25, bon26, bon27, bon28, bon29, bon30, bon31, bon32, bon33, bon34, bon35, bon36, bon37, bon38, bon39, bon40
                };

                _dbContext.Items.AddRange(bonnen);

                User user00 = new User { FirstName = "John", FamilyName = "Doe", Sex = Sex.Different, EmailAddress = "lekkerlokaal" };
                User user01 = new User { FirstName = "Brent", FamilyName = "Schets", Sex = Sex.Male, EmailAddress = "brent@schets.com" };
                User user02 = new User { FirstName = "Bram", FamilyName = "De Coninck", Sex = Sex.Male, EmailAddress = "bram@bramdeconinck.com" };
                User user03 = new User { FirstName = "Lennert", FamilyName = "Bontinck", Sex = Sex.Male, EmailAddress = "lennert@lennertbontinck.com" };

                var personen = new List<User>
                {
                    user00, user01, user02, user03
                };

                _dbContext.User.AddRange(personen);

                _dbContext.SaveChanges();

                //admin voorbeeld
                await CreateUser("lekkerlokaalst@gmail.com", "lekkerlokaalst@gmail.com", "BraLenBreAdmin", "admin");
                await CreateUser("admin@lekkerlokaal.be", "admin@lekkerlokaal.be", "BraLenBreProductions", "admin");
                await CreateUser("klant@gmail.com", "klant@gmail.com", "klantje", "customers");
                _dbContext.User.Add(new User
                {
                    EmailAddress = "lekkerlokaalst@gmail.com",
                    FirstName = "BraLenBre",
                    FamilyName = "Groep één",
                    Sex = Sex.Male
                });
                _dbContext.User.Add(new User
                {
                    EmailAddress = "admin@lekkerlokaal.be",
                    FirstName = "Joachim",
                    FamilyName = "Rummens",
                    Sex = Sex.Male
                });
                _dbContext.User.Add(new User
                {
                    EmailAddress = "klant@gmail.com",
                    FirstName = "Klant",
                    FamilyName = "Janssens",
                    Sex = Sex.Male
                });

                _dbContext.SaveChanges();

                //seller voorbeeld
                var user = new ApplicationUser { UserName = "info@lokaalmarkt.be", Email = "info@lokaalmarkt.be", EmailConfirmed = true };
                var wachtwoord = "BraLenBreProductions";
                var result = await _userManager.CreateAsync(user, wachtwoord);
                await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "seller"));

                Seller lokaalmarkt = new Seller("Lokaal", "info@lokaalmarkt.be", "Lokaal is een overdekte boerenmarkt met bar en kinderatelier.", "Denderstraat", "22", "9300", "Aalst", true);
                _dbContext.Seller.Add(lokaalmarkt);

                Items lokaalmarktItems = new Items("Lokaalmarkt Aalst", 15, "De beste markt van aalst!", 27, @"images\items\71\", converse, "Denderstraat", "22", "9300", "Aalst", lokaalmarkt, Offer.No, true);
                _dbContext.Items.Add(lokaalmarktItems);

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
