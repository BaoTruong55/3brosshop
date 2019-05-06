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
                Category converse = new Category("converse", "fa-utensils");
                Category nike = new Category("nike", "fa-heartbeat");
                Category adidas = new Category("adidas", "fa-plane");
                Category palladium = new Category("palladium", "fa-home");
                Category vans = new Category("vans", "fa-calendar");
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


                Seller Handelaar01 = new Seller("Restaurant Lekker", "lekker@gmail.com", "Met deze receipt kan u lekker komen eten in ons restaurant genaamd Restaurant Lekker.",  "Arbeidstraat", "14", "9300", "Aalst", true);
                Seller Handelaar02 = new Seller("Bontinck", "bontinck@gmail.com", "Met deze receipt kan u onze met passie gemaakte dessertjes komen proeven.",  "Paepestraat", "178", "9260", "Wichelen", true);
                Seller Handelaar03 = new Seller("Schets", "schets@gmail.com", "Alle lokale bieren zijn hier te vinden! Er kan ook plaatselijk geproefd worden.",  "Ravensteinstraat", "50", "1000", "Brussel", true);
                Seller Handelaar04 = new Seller("De Coninck's", "coninck@gmail.com", "De lekkerste cocktails zijn hier te vinden. Alleen hier te vinden tegen een goed prijs!",  "Sint-Pietersnieuwstraat", "124", "9000", "Gent", true);
                Seller Handelaar05 = new Seller("Wijnproeverij BraLenBre", "Handelaar01@gmail.com", "Met deze receipt kan je bij wijnproeverij BraLenBre genieten van een gezellige avond. Je zal er meer uitleg krijgen over de verschillende soorten wijnen en van elke soort mogen proeven, allen vergezeld met een passend hapje. Eens de sessie over is kan met de receipt, wijn gekocht worden. Enkele merken die je hier kan verwachten zijn: Francis Ford Coppola, Franschhoek Cellar, Fushs Reinhardt, Gran Sasso, Grande Provence, Guadalupe, Guillamen I Muri, ..."
                    ,  "Arbeidstraat", "14", "9300", "Aalst", true);
                Seller Handelaar06 = new Seller("'t Sandwichke", "sandwich@gmail.com", "Voor al uw vegatarische noden kan u bij ons terecht.",  "Arbeidstraat", "14", "9300", "Aalst", true);
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

                Seller Handelaar31 = new Seller("Brenk", "brenk@gmail.com", "Stel zelf uw setje bloemen samen met deze receipt.",  "Arbeidstraat", "14", "9300", "Aalst", true);
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

                Receipt bon01 = new Receipt("Restaurant lekker", 25, "3 sterren resaurant in het centrum van Aalst.", 17, @"images\receipt\1\", converse, "Arbeidstraat", "14", "9300", "Aalst", Handelaar01, Offer.No, true);
                Receipt bon02 = new Receipt("Dessertbar chez Bontinck", 5,  "Met passie gemaakte dessertjes in het mooie Schellebelle.", 242, @"images\receipt\2\", converse, "Paepestraat", "178", "9260", "Wichelen", Handelaar02, Offer.Slider, true);
                Receipt bon03 = new Receipt("Bierspecialist Schets", 10, "Meer dan 70 Belgische bieren in een gezellige kroeg.", 42, @"images\receipt\3\", converse, "Ravensteinstraat", "50", "1000", "Brussel", Handelaar03, Offer.No, true);
                Receipt bon04 = new Receipt("De Coninck's cocktail", 5,  "Een VIP cocktailbar met live optredens van lokale muzikanten.", 24, @"images\receipt\4\", converse, "Sint-Pietersnieuwstraat", "124", "9000", "Gent", Handelaar04, Offer.No, true);
                Receipt bon05 = new Receipt("Wijnproeverij BraLenBre", 45, "Keuze uit verschillende wijnen vergezeld met een hapje.", 124, @"images\receipt\5\", converse, "Arbeidstraat", "14", "9300", "Aalst", Handelaar05, Offer.Slider, true);
                Receipt bon06 = new Receipt("Veggiebar 't Sandwichke", 15,  "Het bewijs dat vegetarisch eten lekker kan zijn.", 45, @"images\receipt\6\", converse, "Arbeidstraat", "14", "9300", "Aalst", Handelaar06, Offer.No, true);
                Receipt bon07 = new Receipt("Fastfood McDonalds", 1,  "De keten met keuzes voor iedereen.", 98, @"images\receipt\7\", converse, "Arbeidstraat", "14", "9300", "Aalst", Handelaar07, Offer.No, true);
                Receipt bon08 = new Receipt("Restaurant SOS Piet", 75,  "5 sterren restaurant met de enige echte SOS Piet als kok.", 21, @"images\receipt\8\", converse, "Paepestraat", "178", "9260", "Wichelen", Handelaar08, Offer.No, true);
                Receipt bon09 = new Receipt("Wijnproeverij CoBoSh", 25,  "Keuze uit verschillende wijnen vergezeld met een hapje.", 47, @"images\receipt\9\", converse, "Arbeidstraat", "14", "9300", "Aalst", Handelaar09, Offer.No, true);
                Receipt bon10 = new Receipt("Wijnproeverij chacha", 22,  "Hapje drankje favoriet muziekje.", 22, @"images\receipt\10\", converse, "Ravensteinstraat", "50", "1000", "Brussel", Handelaar10, Offer.No, true);


                Receipt bon11 = new Receipt("Sanitas Wichelen", 5,  "Ideale nike voor oud en jong", 83, @"images\receipt\11\", nike, "Paepestraat", "178", "9260", "Wichelen", Handelaar11, Offer.No, true);
                Receipt bon12 = new Receipt("nike Basic-Fit Aalst", 5,  "Bekenste nike van België met vestigingen over het hele land.", 75, @"images\receipt\12\", nike, "Arbeidstraat", "14", "9300", "Aalst", Handelaar12, Offer.No, true);
                Receipt bon13 = new Receipt("nike Basic-Fit Gent", 4,  "Bekenste nike van België met vestigingen over het hele land.", 8, @"images\receipt\13\", nike, "Sint-Pietersnieuwstraat", "124", "9000", "Gent", Handelaar12, Offer.No, true);
                Receipt bon14 = new Receipt("nike Basic-Fit Brussel", 6, "Bekenste nike van België met vestigingen over het hele land.", 53, @"images\receipt\14\", nike, "Ravensteinstraat", "50", "1000", "Brussel", Handelaar12, Offer.No, true);
                Receipt bon15 = new Receipt("nike Basic-Fit Brugge", 8, "Bekenste nike van België met vestigingen over het hele land.", 53, @"images\receipt\15\", nike, "Maalse Steenweg", "50", "8310", "Brugge", Handelaar12, Offer.No, true);
                Receipt bon16 = new Receipt("nike Basic-Fit Sint-Truiden", 5, "Bekenste nike van België met vestigingen over het hele land.", 72, @"images\receipt\16\", nike, "Luikersteenweg ", "40", "3800", "Sint-Truiden", Handelaar12, Offer.No, true);
                Receipt bon17 = new Receipt("nike Basic-Fit Wetteren", 6, "Bekenste nike van België met vestigingen over het hele land.", 65, @"images\receipt\17\", nike, "Cooppallaan ", "40", "9230", "Wetteren", Handelaar12, Offer.No, true);
                Receipt bon18 = new Receipt("nike Basic-Fit Wichelen", 4,  "Bekenste nike van België met vestigingen over het hele land.", 22, @"images\receipt\18\", nike, "Paepestraat", "178", "9260", "Wichelen", Handelaar12, Offer.No, true);
                Receipt bon19 = new Receipt("nike Basic-Fit Lede", 8, "Bekenste nike van België met vestigingen over het hele land.", 75, @"images\receipt\19\", nike, "Kasteeldreef", "15", "9340", "Lede", Handelaar12, Offer.No, true);


                Receipt bon20 = new Receipt("Nachtwandeling Aalst at night", 5, "Geniet van de sterrenhemel in de mooie streken van Aalst (met gids).", 63, @"images\receipt\20\", adidas, "Arbeidstraat", "14", "9300", "Aalst", Handelaar21, Offer.No, true);
                Receipt bon21 = new Receipt("Dagje wallibi in Dendermonde", 25,  "Wat is er nu leuker dan een dagje wallibi met vrienden", 34, @"images\receipt\21\", adidas, "Mechelsesteenweg ", "138", "9200", "Dendermonde", Handelaar22, Offer.No, true);
                Receipt bon22 = new Receipt("Met de trein naar Oostende", 36,  "Spring zong er al over dus wat houd je tegen het te doen", 45, @"images\receipt\22\", adidas, "Torhoutsesteenweg", "611", "8400", "Oostende", Handelaar23, Offer.No, true);
                Receipt bon23 = new Receipt("Weekendje disneyland parijs", 29,  "Disneyland de bestemming voor groot en klein", 35, @"images\receipt\23\", adidas, "Leopoldlaan", "1", "1930", "Zaventem", Handelaar24, Offer.No, true);
                Receipt bon24 = new Receipt("De grotten van Han", 50,  "Het liedje zit ongetwijfeld al in je hoofd dus ga nu gewoon", 86, @"images\receipt\24\", adidas, "Rue Joseph Lamotte", "2", "5580", "Han-sur-Lesse", Handelaar25, Offer.No, true);
                Receipt bon25 = new Receipt("Historisch bezoek Breemdonk", 6, "Voor de oorlog fanaten een must", 35, @"images\receipt\25\", adidas, "Brandstraat", "57", "2830", "Willebroek", Handelaar26, Offer.No, true);


                Receipt bon26 = new Receipt("Bloemencenter Brenk", 1,  "Stel zelf uw setje bloemen samen met deze receipt.", 43, @"images\receipt\26\", palladium, "Arbeidstraat", "14", "9300", "Aalst", Handelaar31, Offer.No, true);
                Receipt bon27 = new Receipt("Potgrond De Mol in Lede", 22, "Heb je grond nodig voor in een pot", 68, @"images\receipt\27\", palladium, "Kasteeldreef", "15", "9340", "Lede", Handelaar32, Offer.No, true);
                Receipt bon28 = new Receipt("Schelfhout Ten Aalst", 36,  "Schelfhout, waar moet je andes zijn!", 75, @"images\receipt\28\", palladium, "Arbeidstraat", "14", "9300", "Aalst", Handelaar33, Offer.No, true);
                Receipt bon29 = new Receipt("Bloemetje liesje in Gent", 13,  "Lies, verkoopt ook wel een madelief ", 25, @"images\receipt\29\", palladium, "Sint-Pietersnieuwstraat", "124", "9000", "Gent", Handelaar34, Offer.No, true);
                Receipt bon30 = new Receipt("Funa Lima tuincentrum Lede", 26,  "Vissen, fonteinen, dieraccesoire...", 14, @"images\receipt\30\", palladium, "Kasteeldreef", "15", "9340", "Lede", Handelaar35, Offer.No, true);
                Receipt bon31 = new Receipt("Vijvervoorziening Blub", 30,  "Blub, de winkel voor vis enthousiasten", 35, @"images\receipt\31\", palladium, "Arbeidstraat", "14", "9300", "Aalst", Handelaar36, Offer.No, true);
                Receipt bon32 = new Receipt("Grasmaaiers Bontinck", 25, "Jaren ervaring in het snoeien", 76, @"images\receipt\32\", palladium, "Paepestraat", "178", "9260", "Wichelen", Handelaar37, Offer.No, true);
                Receipt bon33 = new Receipt("Aveve boerenbond te Aalst", 31, "Bij de boerenbond vind je altijd wat je zoekt", 75, @"images\receipt\33\", palladium, "Arbeidstraat", "14", "9300", "Aalst", Handelaar38, Offer.No, true);
                Receipt bon34 = new Receipt("Groener Gras In Wetteren", 12, "Gazon voorzieningen", 14, @"images\receipt\34\", palladium, "Cooppallaan ", "40", "9230", "Wetteren", Handelaar39, Offer.No, true);

                Receipt bon35 = new Receipt("Pukkelpop weekend tickets", 21,  "Pukkelpop, dat moet je gedaan hebben", 57, @"images\receipt\35\", vans, "Paepestraat", "178", "9260", "Wichelen", Handelaar41, Offer.No, true);
                Receipt bon36 = new Receipt("Lokerse bierfeesten", 44, "Bierfanaten kunnen dit niet missen", 75, @"images\receipt\36\", vans, "Kleine Dam", "1", "9160", "Lokeren", Handelaar42, Offer.No, true);
                Receipt bon37 = new Receipt("Gentse feesten eetfestijn", 42,  "Drinken en eten, meer moet dat niet zijn", 25, @"images\receipt\37\", vans, "Sint-Pietersnieuwstraat", "124", "9000", "Gent", Handelaar43, Offer.No, true);
                Receipt bon38 = new Receipt("Gameforce in de Nekkerhalle", 38, "Voor elke nerd wat wils", 14, @"images\receipt\38\", vans, "Ravensteinstraat", "50", "1000", "Brussel", Handelaar44, Offer.No, true);
                Receipt bon39 = new Receipt("Drive A Ferrari Day", 8,  "Ideal geshenk voor een autofanaat", 38, @"images\receipt\39\", vans, "Paepestraat", "178", "9260", "Wichelen", Handelaar45, Offer.No, true);
                Receipt bon40 = new Receipt("Facts: trein en eten", 34,  "Cosplay, eten en vervoer", 18, @"images\receipt\40\", vans, "Sint-Pietersnieuwstraat", "124", "9000", "Gent", Handelaar46, Offer.No, true);

//                Receipt bon41 = new Receipt("Makeup pallete Nude", 29, "Het bekendste merk zijn palette", 67, @"images\receipt\41\", beauty, "Paepestraat", "178", "9260", "Wichelen", Handelaar51, Offer.No, true);
//                Receipt bon42 = new Receipt("Ici Paris verwenbon", 15, "Een parfum kan je nooit mee misdoen", 17, @"images\receipt\42\", beauty, "Arbeidstraat", "14", "9300", "Aalst", Handelaar52, Offer.No, true);
//                Receipt bon43 = new Receipt("Lipstick Lover Aalst", 8,  "Voor de lippen lovers", 86, @"images\receipt\43\", beauty, "Arbeidstraat", "14", "9300", "Aalst", Handelaar53, Offer.No, true);
//
//                Receipt bon44 = new Receipt("Sofa en Co", 18, "Relax in een sofa van sofa en co", 71, @"images\receipt\44\", interieur, "Arbeidstraat", "14", "9300", "Aalst", Handelaar61, Offer.No, true);
//                Receipt bon45 = new Receipt("Deba meubelen", 36,  "Verkoopt al uw interieur", 37, @"images\receipt\45\", interieur, "Arbeidstraat", "14", "9300", "Aalst", Handelaar62, Offer.No, true);
//                Receipt bon46 = new Receipt("Ikea huisvoorzieningen", 40,  "Meubelspiaclist sinds 74", 71, @"images\receipt\46\", interieur, "Paepestraat", "178", "9260", "Wichelen", Handelaar63, Offer.No, true);
//                Receipt bon47 = new Receipt("Leenbakker", 34,  "Om te kopen, niet te lenen", 37, @"images\receipt\47\", interieur, "Sint-Pietersnieuwstraat", "124", "9000", "Gent", Handelaar64, Offer.No, true);
//                Receipt bon48 = new Receipt("Salon Ballon Gent", 13,  "Sallon Ballon is een speciaalzaak te Gent", 76, @"images\receipt\48\", interieur, "Sint-Pietersnieuwstraat", "124", "9000", "Gent", Handelaar65, Offer.No, true);
//                Receipt bon49 = new Receipt("Keukens Donald", 45, "Al 8 jaar maak ik keukens alsof ze voor mezelf zijn", 46, @"images\receipt\49\", interieur, "Arbeidstraat", "14", "9300", "Aalst", Handelaar66, Offer.No, true);
//                Receipt bon50 = new Receipt("Modern Gent", 31,  "Modern interieur hoeft niet duur te zijn", 75, @"images\receipt\50\", interieur, "Sint-Pietersnieuwstraat", "124", "9000", "Gent", Handelaar67, Offer.No, true);
//                Receipt bon51 = new Receipt("Kunst & Kitch", 18,  "Een beetje kunst, een beetje kitch", 46, @"images\receipt\51\", interieur, "Paepestraat", "178", "9260", "Wichelen", Handelaar68, Offer.No, true);
//                Receipt bon52 = new Receipt("Moderne interieur Gill", 5,  "Op maat gemaakt interieur tegen een zacht prijsje", 45, @"images\receipt\52\", interieur, "Arbeidstraat", "14", "9300", "Aalst", Handelaar69, Offer.No, true);
//
//                Receipt bon53 = new Receipt("C&A Aalst", 24,  "De kledingwinkel in Aalst en omstreken", 42, @"images\receipt\53\", kledij, "Arbeidstraat", "14", "9300", "Aalst", Handelaar71, Offer.No, true);
//                Receipt bon54 = new Receipt("AS Advantures", 48,  "Kledie om een avontuur mee aan te gaan", 47, @"images\receipt\54\", kledij, "Paepestraat", "178", "9260", "Wichelen", Handelaar72, Offer.No, true);
//                Receipt bon55 = new Receipt("Ultra Wet", 41,  "De kledingspecialist voor droog en nat", 71, @"images\receipt\55\", kledij, "Arbeidstraat", "14", "9300", "Aalst", Handelaar73, Offer.No, true);
//                Receipt bon56 = new Receipt("Holiday", 24,  "Voor al uw feestkledij", 17, @"images\receipt\56\", kledij, "Sint-Pietersnieuwstraat", "124", "9000", "Gent", Handelaar74, Offer.No, true);
//                Receipt bon57 = new Receipt("Bram's Fashion", 23, "Voor ieder wat wilds", 73, @"images\receipt\57\", kledij, "Paepestraat", "178", "9260", "Wichelen", Handelaar75, Offer.Standard, true);
//                Receipt bon58 = new Receipt("Bontinck's Panthers", 30,  "Pants from Bontinck are dreams for legs", 72, @"images\receipt\58\", kledij, "Arbeidstraat", "14", "9300", "Aalst", Handelaar76, Offer.No, true);
//                Receipt bon59 = new Receipt("Bre Bra", 29,  "Van A tot Z, u vindt het bij ons", 92, @"images\receipt\59\", kledij, "Sint-Pietersnieuwstraat", "124", "9000", "Gent", Handelaar77, Offer.Standard, true);
//                Receipt bon69 = new Receipt("Pikantje", 34,  "Erotische kledingwinkel", 9, @"images\receipt\69\", kledij, "Arbeidstraat", "14", "9300", "Aalst", Handelaar78 , Offer.No, true);
//
//                Receipt bon61 = new Receipt("Fnac Aalst", 3,  "De multimedia specialist in Europa", 54, @"images\receipt\61\", multimedia, "Arbeidstraat", "14", "9300", "Aalst", Handelaar81, Offer.No, true);
//                Receipt bon62 = new Receipt("Mediamarkt Dendermonde", 46,  "Electronica tegen een spot prijs", 45, @"images\receipt\62\", multimedia, "Mechelsesteenweg", "138", "9200", "Dendermonde", Handelaar82, Offer.No, true);
//                Receipt bon63 = new Receipt("Van Den Borre Gent", 31,  "Koffiezets voor 12€", 15, @"images\receipt\63\", multimedia, "Sint-Pietersnieuwstraat", "124", "9000", "Gent", Handelaar83, Offer.No, true);
//                Receipt bon64 = new Receipt("Bontinck IT brugge", 36,  "Een probleempje groot of klein, dan moet je bij IT Lennert zijn", 67, @"images\receipt\64\", multimedia, "Maalse Steenweg", "50", "8310", "Brugge", Handelaar84, Offer.Standard, true);
//                Receipt bon65 = new Receipt("Schets Apple Premium", 49,  "U vindt alle laatste Apple producten hier", 78, @"images\receipt\65\", multimedia, "Paepestraat", "178", "9260", "Wichelen", Handelaar85, Offer.No, true);
//                Receipt bon66 = new Receipt("Lab9 Aalst", 7,  "Officiele Apple reseller", 64, @"images\receipt\66\", multimedia, "Arbeidstraat", "14", "9300", "Aalst", Handelaar86, Offer.No, true);
//                Receipt bon67 = new Receipt("De Conincks Screen Repair", 38,  "Een ongelukje is rap gebeurd", 75, @"images\receipt\67\", multimedia, "Paepestraat", "178", "9260", "Wichelen", Handelaar87, Offer.No, true);
//                Receipt bon68 = new Receipt("Medion Custom", 40,  "Medion laptop op maat gemaakt", 24, @"images\receipt\68\", multimedia, "Sint-Pietersnieuwstraat", "124", "9000", "Gent", Handelaar88, Offer.No, true);
//                Receipt bon60 = new Receipt("Dell Dinosaur", 27, "MS Dos specialist", 30, @"images\receipt\60\", multimedia, "Paepestraat", "178", "9260", "Wichelen", Handelaar89, Offer.No, true);
//
//
//                Receipt bon70 = new Receipt("Generieke cadeaubon", 1, "Niet zeker welke receipt u juist wilt, dan is deze generieke receipt iets voor u!", 457, @"images\receipt\70\", generiek, "Arbeidstraat", "14", "9300", "Aalst", Handelaar91, Offer.Slider, true);
                var bonnen = new List<Receipt>
                {
                   bon01, bon02, bon03, bon04, bon05, bon06, bon07, bon08, bon09, bon10, bon11, bon12, bon13, bon14, bon15, bon16, bon17, bon18, bon19, bon20, bon21, bon22, bon23, bon24, bon25, bon26, bon27, bon28, bon29, bon30, bon31, bon32, bon33, bon34, bon35, bon36, bon37, bon38, bon39, bon40
                };

                _dbContext.Receipt.AddRange(bonnen);

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

                Receipt lokaalmarktReceipt = new Receipt("Lokaalmarkt Aalst", 15, "De beste markt van aalst!", 27, @"images\receipt\71\", converse, "Denderstraat", "22", "9300", "Aalst", lokaalmarkt, Offer.No, true);
                _dbContext.Receipt.Add(lokaalmarktReceipt);

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
