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

                Seller Seller01 = new Seller("1970s Base Camp Suede", "lekker@gmail.com", "Vải lưới ở lưỡi gà và dây giày giúp sản phẩm trở nên nổi bật hơn",  "Arbeidstraat", "14", "9300", "Aalst", true);
                Seller Seller02 = new Seller("1970s Vintage Canvas", "Itemstinck@gmail.com", "Vẫn là màu vàng best-seller của nhà Converse.",  "Paepestraat", "178", "9260", "Wichelen", true);
                Seller Seller03 = new Seller("1970s Vintage Canvas", "schets@gmail.com", "Tone màu xanh lạ mắt, dễ chịu nhưng không kém phần cá tính.",  "Ravensteinstraat", "50", "1000", "Brussel", true);
                Seller Seller04 = new Seller("Chuck Taylor All Star 1970s", "coninck@gmail.com", "Thiết kế cổ điển của Chuck 1970s với tone đen huyền thoại ",  "Sint-Pietersnieuwstraat", "124", "9000", "Gent", true);
                Seller Seller05 = new Seller("Chuck Taylor All Star 1970s", "Seller01@gmail.com", "Phiên bản cổ thấp màu đen cá tính của Chuck 1970s được giới trẻ nhiệt tình ưu ái",  "Arbeidstraat", "14", "9300", "Aalst", true);
                Seller Seller06 = new Seller("1970s Base Camp Suede", "sandwich@gmail.com", "Vải lưới ở lưỡi gà và dây giày giúp sản phẩm trở nên nổi bật hơn.",  "Arbeidstraat", "14", "9300", "Aalst", true);
                Seller Seller07 = new Seller("McDonalds", "mc@gmail.com", "Voor een snelle hap moet u bij ons zijn!",  "Arbeidstraat", "14", "9300", "Aalst", true);
                Seller Seller08 = new Seller("SOS Piet", "sospiet@gmail.com", "Het echte restaurant van SOS Piet. Altijd de beste maatlijd voor een gezonde prijs!",  "Paepestraat", "178", "9260", "Wichelen", true);
                Seller Seller09 = new Seller("CoBoSh", "cobosh@gmail.com", "Voor de beste wijnen moet je bij ons zijn! Hierbij kan altijd een hapje geserveerd worden.",  "Arbeidstraat", "14", "9300", "Aalst", true);

                Seller Seller11 = new Seller("Sanitas", "sanitas@gmail.com", "Bekenste nike van Wichelen.",  "Paepestraat", "178", "9260", "Wichelen", true);
                Seller Seller12 = new Seller("nike Basic-Fit", "basicfit@gmail.com", "Bekenste nike van België met vestigingen over het hele land.",  "Ravensteinstraat", "50", "1000", "Brussel", true);

                Seller Seller21 = new Seller("Aalst", "aalst@gmail.com", "De recreatiedienst van Aalst staat in voor tal van speciale activiteiten.",  "Arbeidstraat", "14", "9300", "Aalst", true);
                Seller Seller22 = new Seller("Walibi", "walibi@gmail.com", "Een pretpark voor klein en groot.",  "Mechelsesteenweg ", "138", "9200", "Dendermonde", true);
                Seller Seller23 = new Seller("NMBS", "trein@gmail.com", "De spoorwegdienst van België. Staakt liever dan te werken.",  "Torhoutsesteenweg", "611", "8400", "Oostende", true);
                Seller Seller24 = new Seller("Disneyland Paris", "parijs@gmail.com", "Een van de grootste pretparken in Frankrijk.",  "Leopoldlaan", "1", "1930", "Zaventem", true);
                Seller Seller25 = new Seller("Hamme", "hamme@gmail.com", "Stad Hamme",  "Rue Joseph Lamotte", "2", "5580", "Han-sur-Lesse", true);
                Seller Seller26 = new Seller("Breemdonk", "breemdonk@gmail.com", "City Breemdonk",  "Brandstraat", "57", "2830", "Willebroek", true);

                Seller Seller31 = new Seller("Brenk", "brenk@gmail.com", "Stel zelf uw setje bloemen samen met deze items.",  "Arbeidstraat", "14", "9300", "Aalst", true);
                Seller Seller32 = new Seller("De Mol", "molleken@gmail.com", "Heb je grond nodig voor in een pot",  "Kasteeldreef", "15", "9340", "Lede", true);
                Seller Seller33 = new Seller("Schelfhout", "schelfhout@gmail.com", "Schelfhout, waar moet je andes zijn!",  "Arbeidstraat", "14", "9300", "Aalst", true);
                Seller Seller34 = new Seller("Liesje", "lies@gmail.com", "Lies, verkoopt ook wel een madelief",  "Sint-Pietersnieuwstraat", "124", "9000", "Gent", true);
                Seller Seller35 = new Seller("Funa Lima", "funa_lima@gmail.com", "Vissen, fonteinen, dieraccesoire...",  "Kasteeldreef", "15", "9340", "Lede", true);
                Seller Seller36 = new Seller("Blub", "blub@gmail.com", "Blub, de winkel voor vis enthousiasten",  "Arbeidstraat", "14", "9300", "Aalst", true);
                Seller Seller37 = new Seller("G-Itemst", "grasb@gmail.com", "Jaren ervaring in het snoeien van alle gazons",  "Paepestraat", "178", "9260", "Wichelen", true);
                Seller Seller38 = new Seller("Aveve", "aveve@gmail.com", "Bij de boerenItemsd vind je altijd wat je zoekt",  "Arbeidstraat", "14", "9300", "Aalst", true);
                Seller Seller39 = new Seller("Groener Gras", "groengras@gmail.com", "Gazon voorzieningen voor iedereen die een groen gazon wil!",  "Cooppallaan ", "40", "9230", "Wetteren", true);

                Seller Seller41 = new Seller("Pukkelpop", "ppk@gmail.com", "Tickets of coupons voor pukkelpok.",  "Paepestraat", "178", "9260", "Wichelen", true);
                Seller Seller42 = new Seller("Bierfeesten", "bierfeesten@gmail.com", "De veste feesten in Lokeren: De Lokerse Bierfeesten!",  "Kleine Dam", "1", "9160", "Lokeren", true);
                Seller Seller43 = new Seller("Gentse Feesten", "feesten-gent@gmail.com", "Het grootste feest in Gent!",  "Sint-Pietersnieuwstraat", "124", "9000", "Gent", true);
                Seller Seller44 = new Seller("Gameforce", "games@gmail.com", "Grootste game beurs in België. Nu ook kortingsItemsnen verkrijgbaar!",  "Ravensteinstraat", "50", "1000", "Brussel", true);
                Seller Seller45 = new Seller("Garage Ferrari", "ferfer@gmail.com", "Beste cadeau voor een Ferrari liefhebber!",  "Paepestraat", "178", "9260", "Wichelen", true);
                Seller Seller46 = new Seller("Facts", "facts@gmail.com", "Een van de grootste cosplay beurzen van België.",  "Sint-Pietersnieuwstraat", "124", "9000", "Gent", true);

                Seller Seller51 = new Seller("Nude", "nude@gmail.com", "Het bekendste merk voor beauty producten!",  "Paepestraat", "178", "9260", "Wichelen", true);
                Seller Seller52 = new Seller("Restaurant Lekker", "lekker@gmail.com", "korte beschrijving",  "Arbeidstraat", "14", "9300", "Aalst", true);
                Seller Seller53 = new Seller("Ici Paris", "ici-paris@gmail.com", "Voor een parfum moet je bij ons zijn!",  "Arbeidstraat", "14", "9300", "Aalst", true);

                Seller Seller61 = new Seller("Sofa & Co", "sofaco@gmail.com", "Vind de gepaste sofa bij ons!",  "Arbeidstraat", "14", "9300", "Aalst", true);
                Seller Seller62 = new Seller("Deba", "deba@gmail.com", "Voor elk interieur stuk kan u bij ons terecht!",  "Arbeidstraat", "14", "9300", "Aalst", true);
                Seller Seller63 = new Seller("Ikea", "ikea@gmail.com", "Hebt u iets nodig tegen een lage prijs dan kan u altijd bij ons terecht.",  "Paepestraat", "178", "9260", "Wichelen", true);
                Seller Seller64 = new Seller("Leen Bakker", "leen-bakker@gmail.com", "Iets kopen dan bent u op de juiste plaats.",  "Sint-Pietersnieuwstraat", "124", "9000", "Gent", true);
                Seller Seller65 = new Seller("Salon Ballon", "salon-ballon@gmail.com", "De speciaal zaak die u zocht.",  "Sint-Pietersnieuwstraat", "124", "9000", "Gent", true);
                Seller Seller66 = new Seller("Donald", "donald-keukens@gmail.com", "Keuken nodig kom dan bij ons!",  "Arbeidstraat", "14", "9300", "Aalst", true);
                Seller Seller67 = new Seller("Modern Gent", "gent-modern@gmail.com", "Modern interieur tegen een prijsje.",  "Sint-Pietersnieuwstraat", "124", "9000", "Gent", true);
                Seller Seller68 = new Seller("Kunst & Kitch", "kunst-kitch@gmail.com", "Kunst hoeft niet altijd lelijk te zijn.",  "Paepestraat", "178", "9260", "Wichelen", true);
                Seller Seller69 = new Seller("Gill", "gill@gmail.com", "Interieur tegen een prijsje.",  "Arbeidstraat", "14", "9300", "Aalst", true);

                Seller Seller71 = new Seller("C&A", "cena@gmail.com", "De Kleding winkel van Aalst.",  "Arbeidstraat", "14", "9300", "Aalst", true);
                Seller Seller72 = new Seller("AS Adventure", "as-adventure@gmail.com", "Outdoor kleding en alles voor je outdoor ervaring.",  "Paepestraat", "178", "9260", "Wichelen", true);
                Seller Seller73 = new Seller("Ultra Wet", "ultra-wet@gmail.com", "De kldeingspecialist voor droog en nat.",  "Arbeidstraat", "14", "9300", "Aalst", true);
                Seller Seller74 = new Seller("Holiday", "holiday@gmail.com", "Voor al uw feestkledij.",  "Sint-Pietersnieuwstraat", "124", "9000", "Gent", true);
                Seller Seller75 = new Seller("Bram's Fashion", "bram@gmail.com", "Voor ieder wat wilds.",  "Paepestraat", "178", "9260", "Wichelen", true);
                Seller Seller76 = new Seller("Itemstinck Panther's", "panther@gmail.com", "Pants from Itemstinck are dreams for legs.",  "Arbeidstraat", "14", "9300", "Aalst", true);
                Seller Seller77 = new Seller("Bre Bra", "bre-bra@gmail.com", "Van A tot Z  u vindt het bij ons.",  "Sint-Pietersnieuwstraat", "124", "9000", "Gent", true);
                Seller Seller78 = new Seller("Pikantje", "pikant@gmail.com", "Eroiek u vindt het bij ons.",  "Arbeidstraat", "14", "9300", "Aalst", true);

                Seller Seller81 = new Seller("Fnac", "fnac@gmail.com", "De multimedia specialist in Europa.",  "Arbeidstraat", "14", "9300", "Aalst", true);
                Seller Seller82 = new Seller("Mediamarkt", "mediamarkt@gmail.com", "Electronica tegen een spot prijs.",  "Mechelsesteenweg", "138", "9200", "Dendermonde", true);
                Seller Seller83 = new Seller("Van Den Borre", "vandenborre@gmail.com", "Koffiezets voor 12€.",  "Sint-Pietersnieuwstraat", "124", "9000", "Gent", true);
                Seller Seller84 = new Seller("Itemstinck IT", "Itemstinck-it@gmail.com", "Een probleempje groot of klein, dan moet je bij IT Lennert zijn.",  "Maalse Steenweg", "50", "8310", "Brugge", true);
                Seller Seller85 = new Seller("Schets Apple Premium", "schets-apple@gmail.com", "Alle laatste Apple producten moet u bij ons zijn.",  "Paepestraat", "178", "9260", "Wichelen", true);
                Seller Seller86 = new Seller("Lab9", "lab-9@gmail.com", "Officiele Apple reseller.",  "Arbeidstraat", "14", "9300", "Aalst", true);
                Seller Seller87 = new Seller("De Conincks Screen Repair", "screenrepair@gmail.com", "Een ongelukje is snel gebeurd.",  "Paepestraat", "178", "9260", "Wichelen", true);
                Seller Seller88 = new Seller("Medion Custom", "medion@gmail.com", "Medion laptop op maat gemaakt",  "Sint-Pietersnieuwstraat", "124", "9000", "Gent", true);
                Seller Seller89 = new Seller("Dell Dinosaur", "dell@gmail.com", "MS Dos specialist",  "Paepestraat", "178", "9260", "Wichelen", true);


                Seller Seller91 = new Seller("Generiek", "generiek@gmail.com", "generiek",  "Ravensteinstraat", "50", "1000", "Brussel", true);

                Seller Seller10 = new Seller("ChaCha", "chacha@gmail.com", "Voor de beste wijnen moet je bij ons zijn! Hierbij kan altijd een hapje geserveerd worden.",  "Ravensteinstraat", "50", "1000", "Brussel", true);

                _dbContext.Seller.AddRange(Seller01, Seller02, Seller03, Seller04, Seller05, Seller06, Seller07, Seller08, Seller09, Seller10, Seller11, Seller21, Seller22, Seller23, Seller24, Seller25, Seller26, Seller31, Seller32, Seller33, Seller34, Seller35, Seller36, Seller37, Seller38, Seller39, Seller41, Seller42, Seller43, Seller44, Seller45, Seller46, Seller51, Seller52, Seller53, Seller61, Seller62, Seller63, Seller64, Seller65, Seller66, Seller67, Seller68, Seller69, Seller71, Seller72, Seller73, Seller74, Seller75, Seller76, Seller77, Seller78, Seller81, Seller82, Seller83, Seller84, Seller85, Seller86, Seller87, Seller88, Seller89, Seller91);

                Items Items01 = new Items("1970s Base Camp Suede", 1700000, "Vải lưới ở lưỡi gà và dây giày giúp sản phẩm trở nên nổi bật hơn", 17, @"images\items\1\", converse, "Sư Vạn Hạnh", "14", "9300", "Hồ Chí Minh", Seller01, Offer.No, true);
                Items Items02 = new Items("1970s Vintage Canvas", 1700000, "Vẫn là màu vàng best-seller của nhà Converse.", 242, @"images\items\2\", converse, "Paepestraat", "178", "9260", "Wichelen", Seller02, Offer.Slider, true);
                Items Items03 = new Items("1970s Vintage Canvas", 1700000, "Tone màu xanh lạ mắt, dễ chịu nhưng không kém phần cá tính.", 42, @"images\items\3\", converse, "Ravensteinstraat", "50", "1000", "Brussel", Seller03, Offer.No, true);
                Items Items04 = new Items("Chuck Taylor All Star 1970s", 1800000, "Thiết kế cổ điển của Chuck 1970s với tone đen huyền thoại .", 24, @"images\items\4\", converse, "Sint-Pietersnieuwstraat", "124", "9000", "Gent", Seller04, Offer.No, true);
                Items Items05 = new Items("Chuck Taylor All Star 1970s", 1400000, "Phiên bản cổ thấp màu đen cá tính của Chuck 1970s được giới trẻ nhiệt tình ưu ái.", 124, @"images\items\5\", converse, "Arbeidstraat", "14", "9300", "Aalst", Seller05, Offer.Slider, true);
                Items Items06 = new Items("1970s Base Camp Suede", 1500000, "Vải lưới ở lưỡi gà và dây giày giúp sản phẩm trở nên nổi bật hơn.", 45, @"images\items\6\", converse, "Arbeidstraat", "14", "9300", "Aalst", Seller06, Offer.No, true);
                Items Items07 = new Items("Chuck Taylor All Star", 1300000,  "Với 2 tone màu Xanh Navy và Xanh Chàm tươi mát cùng xuất hiện ngay trên đôi giày sẽ giúp bạn có được một sản phẩm không bao giờ lỗi mốt.", 98, @"images\items\7\", converse, "Arbeidstraat", "14", "9300", "Aalst", Seller07, Offer.No, true);
                Items Items08 = new Items("All Star Limo Leather", 1700000,  "Toàn bộ thân giày được làm từ chất liệu da cao cấp cùng tone Xanh Navy mang lại một sự mới mẻ.", 21, @"images\items\8\", converse, "Paepestraat", "178", "9260", "Wichelen", Seller08, Offer.No, true);
                Items Items09 = new Items("Converse Chuck Taylor", 1400000,  "Toàn bộ thân giày được làm từ chất liệu da cao cấp cùng tone Xanh Navy mang lại một sự mới mẻ.", 47, @"images\items\9\", converse, "Arbeidstraat", "14", "9300", "Aalst", Seller09, Offer.No, true);
                Items Items10 = new Items("One Star Sunbaked", 1600000,  "Kiểu dáng CONS One-Star với thiết kế All-White để bạn dễ dàng tận dụng với mọi outfit.", 22, @"images\items\10\", converse, "Ravensteinstraat", "50", "1000", "Brussel", Seller10, Offer.No, true);


                Items Items11 = new Items("Nike x CLOT Air Max Haven", 2200000,  "Ideale nike voor oud en jong", 83, @"images\items\11\", nike, "Paepestraat", "178", "9260", "Wichelen", Seller11, Offer.No, true);
                Items Items12 = new Items("AIR JORDAN 1 RETRO HIGH OG", 2200000,  "Bekenste nike van België met vestigingen over het hele land.", 75, @"images\items\12\", nike, "Arbeidstraat", "14", "9300", "Aalst", Seller12, Offer.No, true);
                Items Items13 = new Items("Air Jordan 11 Retro Low LE", 2200000,  "Bekenste nike van België met vestigingen over het hele land.", 8, @"images\items\13\", nike, "Sint-Pietersnieuwstraat", "124", "9000", "Gent", Seller12, Offer.No, true);
                Items Items14 = new Items("Nike Epic Phantom React", 2200000, "Bekenste nike van België met vestigingen over het hele land.", 53, @"images\items\14\", nike, "Ravensteinstraat", "50", "1000", "Brussel", Seller12, Offer.No, true);
                Items Items15 = new Items("Nike Free RN Flyknit 3.0", 2200000, "Bekenste nike van België met vestigingen over het hele land.", 53, @"images\items\15\", nike, "Maalse Steenweg", "50", "8310", "Brugge", Seller12, Offer.No, true);
                Items Items16 = new Items("Nike Vaporfly 4% Flyknit", 2200000, "Bekenste nike van België met vestigingen over het hele land.", 72, @"images\items\16\", nike, "Luikersteenweg ", "40", "3800", "Sint-Truiden", Seller12, Offer.No, true);
                Items Items17 = new Items("NikeCourt Royale AC SE", 2200000, "Bekenste nike van België met vestigingen over het hele land.", 65, @"images\items\17\", nike, "Cooppallaan ", "40", "9230", "Wetteren", Seller12, Offer.No, true);
                Items Items18 = new Items("LeBron 16 SB", 2200000,  "Bekenste nike van België met vestigingen over het hele land.", 22, @"images\items\18\", nike, "Paepestraat", "178", "9260", "Wichelen", Seller12, Offer.No, true);
                Items Items19 = new Items("Nike Free RN 5.0", 2200000, "Bekenste nike van België met vestigingen over het hele land.", 75, @"images\items\19\", nike, "Kasteeldreef", "15", "9340", "Lede", Seller12, Offer.No, true);


                Items Items20 = new Items("Adidas NMD_CS1 Primeknit", 2500000, "Geniet van de sterrenhemel in de mooie streken van Aalst (met gids).", 63, @"images\items\20\", adidas, "Arbeidstraat", "14", "9300", "Aalst", Seller21, Offer.No, true);
                Items Items21 = new Items("Adidas NMD R1", 2500000,  "Wat is er nu leuker dan een dagje wallibi met vrienden", 34, @"images\items\21\", adidas, "Mechelsesteenweg ", "138", "9200", "Dendermonde", Seller22, Offer.No, true);
                Items Items22 = new Items("Adidas Stansmith", 2500000,  "Spring zong er al over dus wat houd je tegen het te doen", 45, @"images\items\22\", adidas, "Torhoutsesteenweg", "611", "8400", "Oostende", Seller23, Offer.No, true);
                Items Items23 = new Items("Adidas UltraBoost 2.0", 2500000,  "Disneyland de bestemming voor groot en klein", 35, @"images\items\23\", adidas, "Leopoldlaan", "1", "1930", "Zaventem", Seller24, Offer.No, true);
                Items Items24 = new Items("Adidas NMD_CS1 Primeknit", 2500000,  "Het liedje zit ongetwijfeld al in je hoofd dus ga nu gewoon", 86, @"images\items\24\", adidas, "Rue Joseph Lamotte", "2", "5580", "Han-sur-Lesse", Seller25, Offer.No, true);
                Items Items25 = new Items("Adidas ZX 500 RM", 2500000, "Voor de oorlog fanaten een must", 35, @"images\items\25\", adidas, "Brandstraat", "57", "2830", "Willebroek", Seller26, Offer.No, true);


                Items Items26 = new Items("PAMPA OXFORD ORIGINALE", 1600000,  "Stel zelf uw setje bloemen samen met deze items.", 43, @"images\items\26\", palladium, "Arbeidstraat", "14", "9300", "Aalst", Seller31, Offer.No, true);
                Items Items27 = new Items("PAMPALICIOUS POP CORN", 1600000, "Heb je grond nodig voor in een pot", 68, @"images\items\27\", palladium, "Kasteeldreef", "15", "9340", "Lede", Seller32, Offer.No, true);
                Items Items28 = new Items("PAMPALICIOUS STARLIGHT BLUE", 1600000,  "Schelfhout, waar moet je andes zijn!", 75, @"images\items\28\", palladium, "Arbeidstraat", "14", "9300", "Aalst", Seller33, Offer.No, true);
                Items Items29 = new Items("PAMPALICIOUS BLOSSOM", 1600000,  "Lies, verkoopt ook wel een madelief ", 25, @"images\items\29\", palladium, "Sint-Pietersnieuwstraat", "124", "9000", "Gent", Seller34, Offer.No, true);
                Items Items30 = new Items("PALLADIUM PAMPA HI DARE", 1600000,  "Vissen, fonteinen, dieraccesoire...", 14, @"images\items\30\", palladium, "Kasteeldreef", "15", "9340", "Lede", Seller35, Offer.No, true);
                Items Items31 = new Items("PALLADIUM PAMPA HI DARE", 1600000,  "Blub, de winkel voor vis enthousiasten", 35, @"images\items\31\", palladium, "Arbeidstraat", "14", "9300", "Aalst", Seller36, Offer.No, true);
                Items Items32 = new Items("PALLADIUM PAMPA FREE CANVAS", 1600000, "Jaren ervaring in het snoeien", 76, @"images\items\32\", palladium, "Paepestraat", "178", "9260", "Wichelen", Seller37, Offer.No, true);
                Items Items33 = new Items("PAMPA LITE CUFF WATERPROOF", 1600000, "Bij de boerenItemsd vind je altijd wat je zoekt", 75, @"images\items\33\", palladium, "Arbeidstraat", "14", "9300", "Aalst", Seller38, Offer.No, true);
                Items Items34 = new Items("PALLABROUSE BAGGY", 1600000, "Gazon voorzieningen", 14, @"images\items\34\", palladium, "Cooppallaan ", "40", "9230", "Wetteren", Seller39, Offer.No, true);

                Items Items35 = new Items("VANS ERA GET THE REAL", 1700000,  "Pukkelpop, dat moet je gedaan hebben", 57, @"images\items\35\", vans, "Paepestraat", "178", "9260", "Wichelen", Seller41, Offer.No, true);
                Items Items36 = new Items("VANS UA OLD SKOOL I HEART ", 1700000, "Bierfanaten kunnen dit niet missen", 75, @"images\items\36\", vans, "Kleine Dam", "1", "9160", "Lokeren", Seller42, Offer.No, true);
                Items Items37 = new Items("VANS OLD SKOOL OT SIDEWALL", 1700000,  "Drinken en eten, meer moet dat niet zijn", 25, @"images\items\37\", vans, "Sint-Pietersnieuwstraat", "124", "9000", "Gent", Seller43, Offer.No, true);
                Items Items38 = new Items("ANAHEIM FACTORY STYLE 73 DX", 1700000, "Voor elke nerd wat wils", 14, @"images\items\38\", vans, "Ravensteinstraat", "50", "1000", "Brussel", Seller44, Offer.No, true);
                Items Items39 = new Items("CLASSIC COM AUTHENTIC", 1700000,  "Ideal geshenk voor een autofanaat", 38, @"images\items\39\", vans, "Paepestraat", "178", "9260", "Wichelen", Seller45, Offer.No, true);
                Items Items40 = new Items("COMFYCUSH OLD SKOOL", 1700000,  "Cosplay, eten en vervoer", 18, @"images\items\40\", vans, "Sint-Pietersnieuwstraat", "124", "9000", "Gent", Seller46, Offer.No, true);


                _dbContext.Items.AddRange(Items01, Items02, Items03, Items04, Items05, Items06, Items07, Items08, Items09, Items10, Items11, Items12, Items13, Items14, Items15, Items16, Items17, Items18, Items19, Items20, Items21, Items22, Items23, Items24, Items25, Items26, Items27, Items28, Items29, Items30, Items31, Items32, Items33, Items34, Items35, Items36, Items37, Items38, Items39, Items40);

                User user00 = new User { FirstName = "John", FamilyName = "Doe", Sex = Sex.Different, EmailAddress = "lekkerlokaal" };
                User user01 = new User { FirstName = "Brent", FamilyName = "Schets", Sex = Sex.Male, EmailAddress = "brent@schets.com" };
                User user02 = new User { FirstName = "Bram", FamilyName = "De Coninck", Sex = Sex.Male, EmailAddress = "bram@bramdeconinck.com" };
                User user03 = new User { FirstName = "Lennert", FamilyName = "Itemstinck", Sex = Sex.Male, EmailAddress = "lennert@lennertItemstinck.com" };

                _dbContext.User.AddRange(user00, user01, user02, user03);

                _dbContext.SaveChanges();

                //admin example
                await CreateUser("quocbao0505.qt@gmail.com", "quocbao0505.qt@gmail.com", "admin123456", "admin");
                await CreateUser("longvox98@gmail.com", "longvox98@gmail.com", "adminadmin", "admin");
                await CreateUser("nguoidung@gmail.com", "nguoidung@gmail.com", "nguoidung", "customers");
                _dbContext.User.Add(new User
                {
                    EmailAddress = "quocbao0505.qt@gmail.com",
                    FirstName = "Bao",
                    FamilyName = "Quoc",
                    Sex = Sex.Male
                });
                _dbContext.User.Add(new User
                {
                    EmailAddress = "longvox98@gmail.com",
                    FirstName = "Long",
                    FamilyName = "Hoang",
                    Sex = Sex.Male
                });
                _dbContext.User.Add(new User
                {
                    EmailAddress = "nguoidung@gmail.com",
                    FirstName = "Dung",
                    FamilyName = "Nguoi",
                    Sex = Sex.Male
                });

                _dbContext.SaveChanges();

                //seller example
                var user = new ApplicationUser { UserName = "banhang@3brosshop.com", Email = "banhang@3brosshop.com", EmailConfirmed = true };
                var password = "banhang";
                var result = await _userManager.CreateAsync(user, password);

                await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "seller"));

                Seller localMarket = new Seller("BanHang", "banhang@3brosshop.com", "abc", "Denderstraat", "22", "9300", "Aalst", true);
                _dbContext.Seller.Add(localMarket);

                Items localMarketItem = new Items("Lokaalmarkt Aalst", 15, "De beste markt van aalst!", 27, @"images\items\71\", converse, "Denderstraat", "22", "9300", "Aalst", localMarket, Offer.No, true);
                _dbContext.Items.Add(localMarketItem);

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
