using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PEOcleanWPFApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModelsWithNewFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MonthlyRate",
                table: "ServiceAddresses",
                newName: "MonthlyRateJanitor");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "WorkTypes",
                type: "TEXT",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "RequiresPhoto",
                table: "WorkTypes",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "UnitOfMeasure",
                table: "WorkTypes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Apartments",
                table: "ServiceAddresses",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "BuildingType",
                table: "ServiceAddresses",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ConstructionYear",
                table: "ServiceAddresses",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "GarbageChuteType",
                table: "ServiceAddresses",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "HouseArea",
                table: "ServiceAddresses",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MonthlyRateCleaner",
                table: "ServiceAddresses",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ObjectType",
                table: "ServiceAddresses",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Employees",
                type: "TEXT",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Employees",
                type: "INTEGER",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCleaner",
                table: "Employees",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsJanitor",
                table: "Employees",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            // Seed data for ServiceAddresses
            migrationBuilder.InsertData(
                table: "ServiceAddresses",
                columns: new[] { "Id", "Address", "Floors", "Entrances", "Apartments", "HouseArea", "YardArea", "MonthlyRateJanitor", "MonthlyRateCleaner", "ObjectType", "GarbageChuteType", "BuildingType", "ConstructionYear" },
                values: new object[,]
                {
                    { 1, "Комсомольская ул. 36", 5, 3, 60, 3189.8m, 3836m, 1500m, 2000m, "Многоквартирный дом", "отсутствует", "панельный", 1987 },
                    { 2, "Комсомольская ул. 46", 5, 6, 85, 4267.7m, 2051m, 5700m, 5100m, "Многоквартирный дом", "отсутствует", "панельный", 1971 },
                    { 3, "Комсомольская ул. 50", 5, 4, 56, 4180.3m, 1538m, 4000m, 3400m, "Многоквартирный дом", "отсутствует", "кирпичный", 1975 },
                    { 4, "Комсомольская ул. 54", 5, 6, 85, 4212.5m, 2988m, 5700m, 5700m, "Многоквартирный дом", "отсутствует", "панельный", 1972 },
                    { 5, "Комсомольская ул. 59", 9, 1, 36, 1848.2m, 650m, 1000m, 1500m, "Многоквартирный дом", "на лестничной клетке", "панельный", 1979 },
                    { 6, "Комсомольская ул. 60", 5, 10, 149, 7151.4m, 11144m, 9000m, 10000m, "Многоквартирный дом", "отсутствует", "панельный", 1974 },
                    { 7, "Комсомольская ул. 62", 9, 1, 36, 1811.4m, 1030m, 1000m, 1500m, "Многоквартирный дом", "на лестничной клетке", "панельный", 1976 },
                    { 8, "Комсомольская ул. 64", 5, 12, 179, 9210.9m, 7920m, 13000m, 11500m, "Многоквартирный дом", "отсутствует", "панельный", 1975 },
                    { 9, "Комсомольская ул. 72", 5, 6, 85, 4230.6m, 3453m, 7000m, 5100m, "Многоквартирный дом", "отсутствует", "панельный", 1975 },
                    { 10, "Комсомольская ул. 73", 5, 4, 85, 4092.28m, 2742m, 6000m, 3800m, "Многоквартирный дом", "отсутствует", "панельный", 1976 },
                    { 11, "Комсомольская ул. 75", 5, 2, 30, 1361.9m, 2388m, 2000m, 1900m, "Многоквартирный дом", "отсутствует", "панельный", 1976 },
                    { 12, "Комсомольская ул. 83", 9, 1, 36, 1924.1m, 662.5m, 1500m, 1600m, "Многоквартирный дом", "на лестничной клетке", "панельный", 1982 },
                    { 13, "Комсомольская ул. 91", 9, 1, 36, 1834.5m, 366m, 1500m, 1600m, "Многоквартирный дом", "на лестничной клетке", "панельный", 1982 },
                    { 14, "Комсомольская ул. 119", 9, 2, 108, 3859.3m, 4691.95m, 2000m, 3000m, "Многоквартирный дом", "на лестничной клетке", "панельный", 1991 },
                    { 15, "Моторостроителей ул. 52", 9, 1, 36, 1835.2m, 2036m, 1000m, 1300m, "Многоквартирный дом", "на лестничной клетке", "панельный", 1976 },
                    { 16, "Моторостроителей ул. 64", 5, 4, 60, 2797.2m, 3742m, 4000m, 4000m, "Многоквартирный дом", "на лестничной клетке", "кирпичный", 1987 },
                    { 17, "Моторостроителей ул. 69в", 5, 2, 24, 2662.8m, 269.3m, 5000m, 2200m, "Многоквартирный дом", "отсутствует", "кирпичный", 2010 },
                    { 18, "Моторостроителей ул. 70", 5, 5, 75, 3475.2m, 6151m, 4500m, 4000m, "Многоквартирный дом", "на лестничной клетке", "кирпичный", 1991 },
                    { 19, "Моторостроителей ул. 75", 9, 2, 72, 3719.8m, 2839m, 2500m, 3000m, "Многоквартирный дом", "на лестничной клетке", "панельный", 1985 },
                    { 20, "Моторостроителей ул. 77", 9, 5, 180, 9233m, 2700m, 6500m, 7200m, "Многоквартирный дом", "на лестничной клетке", "панельный", 1986 },
                    { 21, "50 летия Победы пр-т 3", 9, 4, 126, 7329.61m, 4190m, 5000m, 6500m, "Многоквартирный дом", "на лестничной клетке", "кирпичный", 1984 },
                    { 22, "50 летия Победы пр-т 5", 9, 3, 108, 5919m, 2047m, 3000m, 4000m, "Многоквартирный дом", "на лестничной клетке", "кирпичный", 1981 },
                    { 23, "50 летия Победы пр-т 7", 9, 1, 32, 2535m, 1298m, 500m, 1600m, "Многоквартирный дом", "на лестничной клетке", "кирпичный", 1980 },
                    { 24, "50 летия Победы пр-т 19", 5, 5, 102, 4818m, 2978m, 7500m, 6500m, "Многоквартирный дом", "на лестничной клетке", "кирпичный", 1983 },
                    { 25, "50 летия Победы пр-т 22", 5, 3, 60, 3266.8m, 927m, 3000m, 3000m, "Многоквартирный дом", "на лестничной клетке", "панельный", 1989 },
                    { 26, "Советская ул. 14", 9, 2, 72, 4024.3m, 995m, 3700m, 3000m, "Многоквартирный дом", "на лестничной клетке", "кирпичный", 1994 },
                    { 27, "Советская ул. 22", 5, 2, 20, 1322.9m, 3844m, 1800m, 1800m, "Многоквартирный дом", "на лестничной клетке", "кирпичный", 1997 },
                    { 28, "Дементьева ул. 18", 9, 1, 36, 1830.3m, 467m, 1500m, 1600m, "Многоквартирный дом", "на лестничной клетке", "панельный", 1980 },
                    { 29, "Р.Люксембург ул. 64", 9, 4, 144, 8053.8m, 7603m, 6500m, 5700m, "Многоквартирный дом", "на лестничной клетке", "панельный", 1992 },
                    { 30, "Пролетарская ул. 1", 3, 1, 15, 940.2m, 1408m, 2500m, 2500m, "Многоквартирный дом", "отсутствует", "индивидуальный", 2018 },
                    { 31, "Пролетарская ул. 1а", 3, 1, 15, 918.1m, null, 2500m, 2500m, "Многоквартирный дом", "отсутствует", "индивидуальный", 2017 },
                    { 32, "Пролетарская ул. 25", 3, 1, 24, 1301.2m, null, 1500m, 1500m, "Многоквартирный дом", "отсутствует", "кирпичный", 2015 },
                    { 33, "В.В. Терешковой ул. 17", 3, 2, 30, 1374.2m, null, 1700m, 1700m, "Многоквартирный дом", "отсутствует", "индивидуальный", 2017 },
                    { 34, "Липовая ул. 3", 4, 2, 45, 2596.5m, 1621m, 1600m, 2400m, "Многоквартирный дом", "отсутствует", "индивидуальный", 2020 },
                    { 35, "Медовая ул. 2", 5, 1, 25, 1623m, 991m, 800m, 1600m, "Многоквартирный дом", "отсутствует", "индивидуальный", 2021 }
                });

            // Seed data for Employees
            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "FullName", "Phone", "Notes", "IsJanitor", "IsCleaner", "IsActive" },
                values: new object[,]
                {
                    { 1, "Горячая Ольга Владимировна", "79605420854", "", true, true, true },
                    { 2, "Курицына Любовь Валерьевна", "79092788643", "", false, true, true },
                    { 3, "Волкова Ольга", "79301193426", "", false, true, true },
                    { 4, "Александр Лобанов", "79201165122", "", true, false, true },
                    { 5, "Виноградова Лидия", "79605319070", "", true, true, true },
                    { 6, "Кузьменко Татьяна", "79962418669, 79038205139", "", false, true, true },
                    { 7, "Юдина Вера Николаевна", "79010534373", "", false, true, true },
                    { 8, "Татьяна Анатольевна", "79109788946", "", false, true, true },
                    { 9, "Смоленов Кирилл", "79201314666", "", true, false, true },
                    { 10, "Латонина Карина", "79806620267", "", false, true, true },
                    { 11, "Догадин Андрей", "79108193869", "", true, false, true },
                    { 12, "Нина Александровна", "79807078029", "", true, true, true },
                    { 13, "Виноградова Нина Викторовна", "79080290269", "", false, true, true },
                    { 14, "Титов Алексей", "79010539737", "", true, true, true },
                    { 15, "Валентина", "79159939196", "", false, true, true },
                    { 16, "Пушкин Олег", "79010556844", "", true, false, true },
                    { 17, "Леонид", "79159813926", "", true, false, true },
                    { 18, "Фролышева Наталья", "79011704508", "", true, true, true },
                    { 19, "Гончарова Юлия", "79807426373", "", false, true, true },
                    { 20, "Гурина Татьяна Владимировна", "79301129153", "", true, false, true },
                    { 21, "Алдухова Наталья", "79806503748, 79997901573", "", true, false, true },
                    { 22, "Лоскутова Юлия Алексеевна", "79807055009", "", true, false, true },
                    { 23, "Татьяна", "79619730734", "", false, true, true },
                    { 24, "Сысоева Жанна", "79108221239, 79611591838", "", false, true, true }
                });

            // Seed data for WorkTypes
            migrationBuilder.InsertData(
                table: "WorkTypes",
                columns: new[] { "Id", "Name", "Code", "UnitOfMeasure", "RequiresPhoto", "Description" },
                values: new object[,]
                {
                    { 1, "Сухое подметание лестничных площадок и маршей", "СПЛМ", 0, false, "Ежедневно, понедельник - суббота" },
                    { 2, "Влажное подметание лестничных площадок и маршей двух нижних этажей", "ВПЛМ2", 0, false, "Ежедневно, понедельник - суббота" },
                    { 3, "Влажное подметание лестничных площадок и маршей выше 2-го этажа", "ВПЛМВ", 0, false, "1 раз в неделю, понедельник" },
                    { 4, "Мытье лестничных площадок и маршей", "МЛМ", 0, false, "1 раз в месяц, 2-3 неделя май - сентябрь" },
                    { 5, "Влажная протирка перил", "ВПЕР", 0, false, "1 раз в месяц" },
                    { 6, "Влажная протирка чердачных лестниц", "ВЧЛ", 0, false, "1 раз в год, сентябрь" },
                    { 7, "Влажная протирка стен", "ВСТ", 0, false, "2 раза в год, май, сентябрь" },
                    { 8, "Влажная протирка шкафов для почтовых ящиков", "ВШПЯ", 0, false, "2 раза в год, май, сентябрь" },
                    { 9, "Влажная протирка шкафов для электросчетчиков", "ВШЭС", 0, false, "2 раза в год, май, сентябрь" },
                    { 10, "Влажная протирка подоконников", "ВПД", 0, false, "1 раз в месяц" },
                    { 11, "Мытье окон", "МОК", 0, false, "2 раза в год, май, сентябрь" },
                    { 12, "Уборка двора", "УДВ", 0, false, "Ежедневно для дворников" },
                    { 13, "Вывоз мусора", "ВМУ", 0, false, "Ежедневно для дворников" },
                    { 14, "Уборка контейнерных площадок", "УКП", 0, false, "Еженедельно для дворников" },
                    { 15, "Уборка подвалов", "УПД", 0, false, "Ежемесячно для дворников" },
                    { 16, "Уборка чердаков", "УЧР", 0, false, "Ежемесячно для дворников" },
                    { 17, "Уборка лифтов", "УЛИ", 0, false, "Ежедневно для уборщиц" },
                    { 18, "Уборка мусоропроводов", "УМП", 0, false, "Еженедельно для уборщиц" },
                    { 19, "Дезинфекция общих помещений", "ДОП", 0, false, "Ежемесячно для уборщиц" },
                    { 20, "Уборка придомовой территории", "УПТ", 0, false, "Ежедневно для дворников" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "WorkTypes",
                keyColumn: "Id",
                keyValues: new object[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 });

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValues: new object[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 });

            migrationBuilder.DeleteData(
                table: "ServiceAddresses",
                keyColumn: "Id",
                keyValues: new object[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35 });

            migrationBuilder.DropColumn(
                name: "Code",
                table: "WorkTypes");

            migrationBuilder.DropColumn(
                name: "RequiresPhoto",
                table: "WorkTypes");

            migrationBuilder.DropColumn(
                name: "UnitOfMeasure",
                table: "WorkTypes");

            migrationBuilder.DropColumn(
                name: "Apartments",
                table: "ServiceAddresses");

            migrationBuilder.DropColumn(
                name: "BuildingType",
                table: "ServiceAddresses");

            migrationBuilder.DropColumn(
                name: "ConstructionYear",
                table: "ServiceAddresses");

            migrationBuilder.DropColumn(
                name: "GarbageChuteType",
                table: "ServiceAddresses");

            migrationBuilder.DropColumn(
                name: "HouseArea",
                table: "ServiceAddresses");

            migrationBuilder.DropColumn(
                name: "MonthlyRateCleaner",
                table: "ServiceAddresses");

            migrationBuilder.DropColumn(
                name: "ObjectType",
                table: "ServiceAddresses");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "IsCleaner",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "IsJanitor",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "MonthlyRateJanitor",
                table: "ServiceAddresses",
                newName: "MonthlyRate");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Employees",
                type: "TEXT",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 200,
                oldNullable: true);
        }
    }
}
