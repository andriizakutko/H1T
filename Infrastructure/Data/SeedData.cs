using Common.Options;
using Domain;
using Domain.Enums;
using Domain.Transport;
using Infrastructure.PasswordHashing;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public static class SeedData
{
    public static async Task SeedSysAdminAndPermissionsData(
        ApplicationDbContext context, 
        IPasswordHashingService passwordHashingService, 
        AdminOptions options)
    {
        if (await context.Users.AnyAsync()) return;
        
        var user = new User()
        {
            FirstName = options.FirstName,
            LastName = options.LastName,
            Email = options.Email,
            Password = passwordHashingService.HashPassword(options.Password, out var salt),
            Salt = salt,
        };

        await context.Users.AddAsync(user);

        var permissions = new List<Permission>
        {
            new() { Name = Authentication.Permissions.User },
            new() { Name = Authentication.Permissions.Moderator },
            new() { Name = Authentication.Permissions.Admin },
            new() { Name = Authentication.Permissions.SysAdmin }
        };
        
        await context.Permissions.AddRangeAsync(permissions);

        var userPermissions = permissions.Select(x => new UserPermission()
        {
            User = user,
            Permission = x
        }).ToList();

        await context.UserPermissions.AddRangeAsync(userPermissions);

        await context.SaveChangesAsync();
    }

    public static async Task SeedModeratorOverviewStatuses(ApplicationDbContext context)
    {
        if(await context.ModeratorOverviewStatuses.AnyAsync()) return;

        var moderatorOverviewStatuses = new List<ModeratorOverviewStatus>()
        {
            new() { Name = ModeratorOverviewStatuses.Waiting.ToString() },
            new() { Name = ModeratorOverviewStatuses.Overviewing.ToString() },
            new() { Name = ModeratorOverviewStatuses.Rejected.ToString() },
            new() { Name = ModeratorOverviewStatuses.Accepted.ToString() },
        };

        await context.AddRangeAsync(moderatorOverviewStatuses);

        await context.SaveChangesAsync();
    }

    public static async Task SeedTransportCommonData(ApplicationDbContext context)
    {
        if(await context.TransportTypes.AnyAsync()) return;
        
        // Passenger type

        var passengerType = new TransportType()
        {
            Name = "Passenger"
        };

        await context.TransportTypes.AddAsync(passengerType);

        var passengerTransportMakes = new List<TransportMake>()
        {
            new() { Name = "Acura" },
            new() { Name = "Alfa Romeo" },
            new() { Name = "Aston Martin" },
            new() { Name = "Audi" },
            new() { Name = "Bentley" },
            new() { Name = "BMW" },
            new() { Name = "Buick" },
            new() { Name = "Cadillac" },
            new() { Name = "Chevrolet" },
            new() { Name = "Chrysler" },
            new() { Name = "Citroen" },
            new() { Name = "Dodge" },
            new() { Name = "Ferrari" },
            new() { Name = "Fiat" },
            new() { Name = "Ford" },
            new() { Name = "Genesis" },
            new() { Name = "GMC" },
            new() { Name = "Honda" },
            new() { Name = "Hyundai" },
            new() { Name = "Infiniti" },
            new() { Name = "Jaguar" },
            new() { Name = "Jeep" },
            new() { Name = "Kia" },
            new() { Name = "Lamborghini" },
            new() { Name = "Land Rover" },
            new() { Name = "Lexus" },
            new() { Name = "Lincoln" },
            new() { Name = "Lotus" },
            new() { Name = "Maserati" },
            new() { Name = "Mazda" },
            new() { Name = "McLaren" },
            new() { Name = "Mercedes-Benz" },
            new() { Name = "MINI" },
            new() { Name = "Mitsubishi" },
            new() { Name = "Nissan" },
            new() { Name = "Pagani" },
            new() { Name = "Peugeot" },
            new() { Name = "Porsche" },
            new() { Name = "Ram" },
            new() { Name = "Renault" },
            new() { Name = "Rolls-Royce" },
            new() { Name = "Saab" },
            new() { Name = "Subaru" },
            new() { Name = "Suzuki" },
            new() { Name = "Tesla" },
            new() { Name = "Toyota" },
            new() { Name = "Volkswagen" },
            new() { Name = "Volvo" }
        };

        await context.TransportMakes.AddRangeAsync(passengerTransportMakes);

        var passengerTransportTypeMakes = passengerTransportMakes
            .Select(make => new TransportTypeMake
                { TransportType = passengerType, TransportMake = make })
            .ToList();

        await context.TransportTypeMakes.AddRangeAsync(passengerTransportTypeMakes);
        
        var passengerTransportBodyTypes = new List<TransportBodyType>()
        {
            new() { Name = "Sedan" },
            new() { Name = "Hatchback" },
            new() { Name = "Coupe" },
            new() { Name = "Convertible" },
            new() { Name = "Station wagon" },
            new() { Name = "SUV (Sport Utility Vehicle)" },
            new() { Name = "Crossover" },
            new() { Name = "Minivan" },
            new() { Name = "Microcar" },
            new() { Name = "Compact car" }
        };

        await context.TransportBodyTypes.AddRangeAsync(passengerTransportBodyTypes);

        var passengerTransportTypeBodyTypes = passengerTransportBodyTypes
            .Select(bodyType => new TransportTypeBodyType
                { TransportType = passengerType, TransportBodyType = bodyType })
            .ToList();

        await context.TransportTypeBodyTypes.AddRangeAsync(passengerTransportTypeBodyTypes);

        var passengerTransportModelsDictionary = new Dictionary<string, List<TransportModel>>
        { 
             { "Acura", new List<TransportModel>()
                 {
                    new() { Name = "ILX" },
                    new() { Name = "MDX" },
                    new() { Name = "NSX" },
                    new() { Name = "RDX" },
                    new() { Name = "RLX" },
                    new() { Name = "TLX" }
                 }
             },
             { "Alfa Romeo", new List<TransportModel>()
                {
                    new() { Name = "4C" },
                    new() { Name = "Giulia" },
                    new() { Name = "Stelvio" }
                } 
             },
            { "Aston Martin", new List<TransportModel>()
                {
                    new() { Name = "DB11" },
                    new() { Name = "DBS Superleggera" },
                    new() { Name = "Vantage" },
                    new() { Name = "DBX" }
                }
            },
            { "Audi", new List<TransportModel>()
                {
                    new() { Name = "A1" },
                    new() { Name = "A3" },
                    new() { Name = "A4" },
                    new() { Name = "A5" },
                    new() { Name = "A6" },
                    new() { Name = "A7" },
                    new() { Name = "A8" },
                    new() { Name = "Q2" },
                    new() { Name = "Q3" },
                    new() { Name = "Q5" },
                    new() { Name = "Q7" },
                    new() { Name = "Q8" },
                    new() { Name = "TT" },
                    new() { Name = "R8" },
                    new() { Name = "e-tron" }
                }
            },
            { "Bentley", new List<TransportModel>()
                {
                    new() { Name = "Bentayga" },
                    new() { Name = "Continental GT" },
                    new() { Name = "Flying Spur" }
                }
            },
            { "BMW", new List<TransportModel>()
                {
                    new() { Name = "1 Series" },
                    new() { Name = "2 Series" },
                    new() { Name = "3 Series" },
                    new() { Name = "4 Series" },
                    new() { Name = "5 Series" },
                    new() { Name = "6 Series" },
                    new() { Name = "7 Series" },
                    new() { Name = "8 Series" },
                    new() { Name = "X1" },
                    new() { Name = "X2" },
                    new() { Name = "X3" },
                    new() { Name = "X4" },
                    new() { Name = "X5" },
                    new() { Name = "X6" },
                    new() { Name = "X7" },
                    new() { Name = "Z4" },
                    new() { Name = "i3" },
                    new() { Name = "i4" },
                    new() { Name = "i8" }
                }
            },
            { "Buick", new List<TransportModel>()
                {
                    new() { Name = "Encore" },
                    new() { Name = "Enclave" },
                    new() { Name = "Envision" },
                    new() { Name = "Regal" }
                }
            },
            { "Cadillac", new List<TransportModel>()
                {
                    new() { Name = "ATS" },
                    new() { Name = "CT4" },
                    new() { Name = "CT5" },
                    new() { Name = "CT6" },
                    new() { Name = "XT4" },
                    new() { Name = "XT5" },
                    new() { Name = "XT6" },
                    new() { Name = "Escalade" }
                }
            },
            { "Chevrolet", new List<TransportModel>()
                {
                    new() { Name = "Blazer" },
                    new() { Name = "Camaro" },
                    new() { Name = "Corvette" },
                    new() { Name = "Equinox" },
                    new() { Name = "Malibu" },
                    new() { Name = "Silverado" },
                    new() { Name = "Suburban" },
                    new() { Name = "Tahoe" },
                    new() { Name = "Trailblazer" },
                    new() { Name = "Traverse" },
                    new() { Name = "Trax" }
                }
            },
            { "Chrysler", new List<TransportModel>()
                {
                    new() { Name = "300" },
                    new() { Name = "Pacifica" },
                    new() { Name = "Voyager" }
                }
            },
            { "Citroen", new List<TransportModel>()
                {
                    new() { Name = "C3" },
                    new() { Name = "C4" },
                    new() { Name = "C5" },
                    new() { Name = "Cactus" },
                    new() { Name = "Berlingo" },
                    new() { Name = "SpaceTourer" }
                }
            },
            { "Dodge", new List<TransportModel>()
                {
                    new() { Name = "Challenger" },
                    new() { Name = "Charger" },
                    new() { Name = "Durango" },
                    new() { Name = "Grand Caravan" },
                    new() { Name = "Journey" }
                }
            },
            { "Ferrari", new List<TransportModel>()
                {
                    new() { Name = "488" },
                    new() { Name = "812 Superfast" },
                    new() { Name = "F8 Tributo" },
                    new() { Name = "Portofino" },
                    new() { Name = "Roma" },
                    new() { Name = "SF90 Stradale" }
                }
            },
            { "Fiat", new List<TransportModel>()
                {
                    new() { Name = "500" },
                    new() { Name = "500X" },
                    new() { Name = "500L" },
                    new() { Name = "124 Spider" },
                    new() { Name = "Panda" },
                    new() { Name = "Tipo" }
                }
            },
            { "Ford", new List<TransportModel>()
                {
                    new() { Name = "Bronco" },
                    new() { Name = "Edge" },
                    new() { Name = "Escape" },
                    new() { Name = "Expedition" },
                    new() { Name = "Explorer" },
                    new() { Name = "Fiesta" },
                    new() { Name = "Focus" },
                    new() { Name = "Fusion" },
                    new() { Name = "Mustang" },
                    new() { Name = "Ranger" },
                    new() { Name = "Transit" }
                }
            },
            { "Genesis", new List<TransportModel>()
                {
                    new() { Name = "G70" },
                    new() { Name = "G80" },
                    new() { Name = "G90" }
                }
            },
            { "GMC", new List<TransportModel>()
                {
                    new() { Name = "Acadia" },
                    new() { Name = "Canyon" },
                    new() { Name = "Sierra" },
                    new() { Name = "Terrain" },
                    new() { Name = "Yukon" }
                }
            },
            { "Honda", new List<TransportModel>()
                {
                    new() { Name = "Accord" },
                    new() { Name = "Civic" },
                    new() { Name = "CR-V" },
                    new() { Name = "Fit" },
                    new() { Name = "HR-V" },
                    new() { Name = "Insight" },
                    new() { Name = "Odyssey" },
                    new() { Name = "Passport" },
                    new() { Name = "Pilot" },
                    new() { Name = "Ridgeline" }
                }
            },
            { "Hyundai", new List<TransportModel>()
                {
                    new() { Name = "Accent" },
                    new() { Name = "Elantra" },
                    new() { Name = "Ioniq" },
                    new() { Name = "Kona" },
                    new() { Name = "Nexo" },
                    new() { Name = "Palisade" },
                    new() { Name = "Santa Fe" },
                    new() { Name = "Sonata" },
                    new() { Name = "Tucson" },
                    new() { Name = "Veloster" },
                    new() { Name = "Venue" }
                }
            },
            { "Infiniti", new List<TransportModel>()
                {
                    new() { Name = "Q50" },
                    new() { Name = "Q60" },
                    new() { Name = "QX50" },
                    new() { Name = "QX55" },
                    new() { Name = "QX60" },
                    new() { Name = "QX80" }
                }
            },
            { "Jaguar", new List<TransportModel>()
                {
                    new() { Name = "E-PACE" },
                    new() { Name = "F-PACE" },
                    new() { Name = "F-TYPE" },
                    new() { Name = "XE" },
                    new() { Name = "XF" },
                    new() { Name = "XJ" }
                }
            },
            { "Jeep", new List<TransportModel>()
                {
                    new() { Name = "Cherokee" },
                    new() { Name = "Compass" },
                    new() { Name = "Gladiator" },
                    new() { Name = "Grand Cherokee" },
                    new() { Name = "Renegade" },
                    new() { Name = "Wrangler" }
                }
            },
            { "Kia", new List<TransportModel>()
                {
                    new() { Name = "Forte" },
                    new() { Name = "K5" },
                    new() { Name = "Niro" },
                    new() { Name = "Optima" },
                    new() { Name = "Rio" },
                    new() { Name = "Seltos" },
                    new() { Name = "Sorento" },
                    new() { Name = "Soul" },
                    new() { Name = "Sportage" },
                    new() { Name = "Stinger" }
                }
            },
            { "Lamborghini", new List<TransportModel>()
                {
                    new() { Name = "Aventador" },
                    new() { Name = "Huracan" },
                    new() { Name = "Urus" }
                }
            },
            { "Land Rover", new List<TransportModel>()
                {
                    new() { Name = "Defender" },
                    new() { Name = "Discovery" },
                    new() { Name = "Discovery Sport" },
                    new() { Name = "Range Rover" },
                    new() { Name = "Range Rover Evoque" },
                    new() { Name = "Range Rover Sport" },
                    new() { Name = "Range Rover Velar" }
                }
            },
            { "Lexus", new List<TransportModel>()
                {
                    new() { Name = "ES" },
                    new() { Name = "GS" },
                    new() { Name = "GX" },
                    new() { Name = "IS" },
                    new() { Name = "LC" },
                    new() { Name = "LS" },
                    new() { Name = "LX" },
                    new() { Name = "NX" },
                    new() { Name = "RC" },
                    new() { Name = "RX" },
                    new() { Name = "UX" }
                }
            },
            { "Lincoln", new List<TransportModel>()
                {
                    new() { Name = "Aviator" },
                    new() { Name = "Continental" },
                    new() { Name = "Corsair" },
                    new() { Name = "MKZ" },
                    new() { Name = "Nautilus" },
                    new() { Name = "Navigator" }
                }
            },
            { "Lotus", new List<TransportModel>()
                {
                    new() { Name = "Elise" },
                    new() { Name = "Evora" },
                    new() { Name = "Exige" }
                }
            },
            { "Maserati", new List<TransportModel>()
                {
                    new() { Name = "Ghibli" },
                    new() { Name = "Levante" },
                    new() { Name = "Quattroporte" }
                }
            },
            { "Mazda", new List<TransportModel>()
                {
                    new() { Name = "CX-3" },
                    new() { Name = "CX-30" },
                    new() { Name = "CX-5" },
                    new() { Name = "CX-9" },
                    new() { Name = "Mazda2" },
                    new() { Name = "Mazda3" },
                    new() { Name = "Mazda6" },
                    new() { Name = "MX-5 Miata" }
                }
            },
            { "McLaren", new List<TransportModel>()
                {
                    new() { Name = "570S" },
                    new() { Name = "600LT" },
                    new() { Name = "720S" },
                    new() { Name = "GT" }
                }
            },
            { "Mercedes-Benz", new List<TransportModel>()
                {
                    new() { Name = "A-Class" },
                    new() { Name = "C-Class" },
                    new() { Name = "E-Class" },
                    new() { Name = "S-Class" },
                    new() { Name = "CLA" },
                    new() { Name = "CLS" },
                    new() { Name = "GLA" },
                    new() { Name = "GLB" },
                    new() { Name = "GLC" },
                    new() { Name = "GLE" },
                    new() { Name = "GLS" },
                    new() { Name = "SLC" },
                    new() { Name = "SL" },
                    new() { Name = "AMG GT" },
                    new() { Name = "EQC" }
                }
            },
            { "MINI", new List<TransportModel>()
                {
                    new() { Name = "Clubman" },
                    new() { Name = "Convertible" },
                    new() { Name = "Countryman" },
                    new() { Name = "Hardtop 2 Door" },
                    new() { Name = "Hardtop 4 Door" }
                }
            },
            { "Mitsubishi", new List<TransportModel>()
                {
                    new() { Name = "Eclipse Cross" },
                    new() { Name = "Mirage" },
                    new() { Name = "Outlander" },
                    new() { Name = "Outlander Sport" }
                }
            },
            { "Nissan", new List<TransportModel>()
                {
                    new() { Name = "Altima" },
                    new() { Name = "Armada" },
                    new() { Name = "Frontier" },
                    new() { Name = "Kicks" },
                    new() { Name = "Leaf" },
                    new() { Name = "Maxima" },
                    new() { Name = "Murano" },
                    new() { Name = "Pathfinder" },
                    new() { Name = "Rogue" },
                    new() { Name = "Sentra" },
                    new() { Name = "Titan" },
                    new() { Name = "Versa" }
                }
            },
            { "Pagani", new List<TransportModel>()
                {
                    new() { Name = "Huayra" },
                    new() { Name = "Huayra Roadster" }
                }
            },
            { "Peugeot", new List<TransportModel>()
                {
                    new() { Name = "108" },
                    new() { Name = "208" },
                    new() { Name = "308" },
                    new() { Name = "508" },
                    new() { Name = "2008" },
                    new() { Name = "3008" },
                    new() { Name = "5008" }
                }
            },
            { "Porsche", new List<TransportModel>()
                {
                    new() { Name = "911" },
                    new() { Name = "718 Boxster" },
                    new() { Name = "718 Cayman" },
                    new() { Name = "Panamera" },
                    new() { Name = "Cayenne" },
                    new() { Name = "Macan" },
                    new() { Name = "Taycan" }
                }
            },
            { "Ram", new List<TransportModel>()
                {
                    new() { Name = "1500" },
                    new() { Name = "2500" },
                    new() { Name = "3500" },
                    new() { Name = "ProMaster City" }
                }
            },
            { "Renault", new List<TransportModel>()
                {
                    new() { Name = "Clio" },
                    new() { Name = "Captur" },
                    new() { Name = "Megane" },
                    new() { Name = "Scenic" },
                    new() { Name = "Kadjar" },
                    new() { Name = "Talisman" },
                    new() { Name = "Koleos" },
                    new() { Name = "Zoe" }
                }
            },
            { "Rolls-Royce", new List<TransportModel>()
                {
                    new() { Name = "Phantom" },
                    new() { Name = "Ghost" },
                    new() { Name = "Wraith" },
                    new() { Name = "Dawn" },
                    new() { Name = "Cullinan" }
                }
            },
            { "Saab", new List<TransportModel>()
                {
                    new() { Name = "9-3" },
                    new() { Name = "9-5" }
                }
            },
            { "Subaru", new List<TransportModel>()
            {
                new() { Name = "Ascent" },
                new() { Name = "Crosstrek" },
                new() { Name = "Forester" },
                new() { Name = "Impreza" },
                new() { Name = "Legacy" },
                new() { Name = "Outback" }
            }
            },
            { "Suzuki", new List<TransportModel>()
                {
                    new() { Name = "Celerio" },
                    new() { Name = "Swift" },
                    new() { Name = "Baleno" },
                    new() { Name = "Ignis" },
                    new() { Name = "Vitara" },
                    new() { Name = "S-Cross" }
                }
            },
            { "Tesla", new List<TransportModel>()
                {
                    new() { Name = "Model 3" },
                    new() { Name = "Model S" },
                    new() { Name = "Model X" },
                    new() { Name = "Model Y" },
                    new() { Name = "Cybertruck" },
                    new() { Name = "Roadster" }
                }
            },
            { "Toyota", new List<TransportModel>()
                {
                    new() { Name = "4Runner" },
                    new() { Name = "Avalon" },
                    new() { Name = "Camry" },
                    new() { Name = "Corolla" },
                    new() { Name = "Highlander" },
                    new() { Name = "Prius" },
                    new() { Name = "RAV4" },
                    new() { Name = "Sienna" },
                    new() { Name = "Tacoma" },
                    new() { Name = "Tundra" }
                }
            },
            { "Volkswagen", new List<TransportModel>()
                {
                    new() { Name = "Arteon" },
                    new() { Name = "Atlas" },
                    new() { Name = "Golf" },
                    new() { Name = "Jetta" },
                    new() { Name = "Passat" },
                    new() { Name = "Tiguan" },
                    new() { Name = "Touareg" }
                }
            },
            { "Volvo", new List<TransportModel>()
                {
                    new() { Name = "S60" },
                    new() { Name = "S90" },
                    new() { Name = "V60" },
                    new() { Name = "V90" },
                    new() { Name = "XC40" },
                    new() { Name = "XC60" },
                    new() { Name = "XC90" }
                }
            }
        };

        foreach (var keyValue in passengerTransportModelsDictionary)
        {
            await context.TransportModels.AddRangeAsync(keyValue.Value);
        }

        foreach (var transportMakeModels in from make in passengerTransportMakes let transportModels = passengerTransportModelsDictionary.GetValueOrDefault(make.Name) select transportModels.Select(model => new TransportMakeModel() { TransportMake = make, TransportModel = model }).ToList())
        {
            await context.TransportMakeModels.AddRangeAsync(transportMakeModels);
        }

        await context.SaveChangesAsync();
    }
}