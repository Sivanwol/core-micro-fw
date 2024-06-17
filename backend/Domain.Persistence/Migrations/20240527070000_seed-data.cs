using Application.Utils;
using Domain.Entities;
using Domain.Persistence.Context;
using Infrastructure.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;
using Newtonsoft.Json;

#nullable disable

namespace Domain.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class seeddata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                            "countries",
                            new[] { "country_name", "country_code", "country_number" },
                            columnTypes: new[] { "nvarchar(max)", "nvarchar(max)", "nvarchar(max)" },
                            new object[,] {
                    //...more countries from America
                    { "United States", "US", "1"},
                    { "Canada", "CA", "1" },
                    { "Mexico", "MX", "52"},
                    { "Argentina", "AR", "54" },
                    { "Brazil", "BR", "55" },
                    { "Chile", "CL", "56" },
                    { "Colombia", "CO", "57" },
                    { "Peru", "PE", "51" },
                    { "Venezuela", "VE", "58" },
                    { "Ecuador", "EC", "593" },
                    { "Uruguay", "UY", "598" },
                    { "Paraguay", "PY", "595" },
                    { "Bolivia", "BO", "591" },
                    { "Guatemala", "GT", "502" },
                    { "Cuba", "CU", "53" },
                    { "Honduras", "HN", "504" },
                    { "Dominican Republic", "DO", "1" },
                    { "El Salvador", "SV", "503" },
                    { "Costa Rica", "CR", "506" },
                    { "Panama", "PA", "507" },
                    { "Nicaragua", "NI", "505" },
                    { "Puerto Rico", "PR", "1" },
                    { "Jamaica", "JM", "1" },
                    { "Trinidad and Tobago", "TT", "1" },
                    { "Haiti", "HT", "509" },
                    { "Bahamas", "BS", "1" },
                    { "Barbados", "BB", "1" },
                    { "Belize", "BZ", "501" },
                    { "Guyana", "GY", "592" },
                    { "Bermuda", "BM", "1" },
                    { "Suriname", "SR", "597" },
                    { "Cayman Islands", "KY", "1" },
                    { "Antigua and Barbuda", "AG", "1" },
                    { "Saint Lucia", "LC", "1" },
                    { "Aruba", "AW", "297" },
                    { "Grenada", "GD", "1" },
                    { "Saint Vincent and the Grenadines", "VC", "1" },
                    { "Turks and Caicos Islands", "TC", "1" },
                    { "British Virgin Islands", "VG", "1" },
                    { "Saint Kitts and Nevis", "KN", "1" },
                    { "Montserrat", "MS", "1" },
                    { "Anguilla", "AI", "1" },
                    { "Saint Martin", "MF", "590" },
                    { "Martinique", "MQ", "596" },
                    { "Guadeloupe", "GP", "590" },
                    { "Saint Barthélemy", "BL", "590" },
                    { "Puerto Rico", "PR", "1" },
                    { "Dominica", "DM", "1" },
                    { "Curaçao", "CW", "599" },
                    { "Bonaire, Sint Eustatius and Saba", "BQ", "599" },
                    { "Sint Maarten", "SX", "1" },
                    { "Saint Pierre and Miquelon", "PM", "508" },
                    { "Greenland", "GL", "299" },
                    { "Falkland Islands", "FK", "500" },
                    { "United States Minor Outlying Islands", "UM", "1" },
                    { "American Samoa", "AS", "1" },
                    { "Guam", "GU", "1" },
                    { "Northern Mariana Islands", "MP", "1" },
                    { "Palau", "PW", "680" },
                    { "Cook Islands", "CK", "682" },
                    { "Tuvalu", "TV", "688" },
                    { "Wallis and Futuna", "WF", "681" },
                    { "Nauru", "NR", "674" },
                    { "Niue", "NU", "683" },
                    { "Tokelau", "TK", "690" },
                    { "Saint Helena", "SH", "290" },
                    { "Ascension Island", "AC", "247" },
                    { "Tristan da Cunha", "TA", "290" },
                    //European countries
                    { "Germany", "DE", "49" },
                    { "France", "FR", "33" },
                    { "United Kingdom", "GB", "44" },
                    { "Italy", "IT", "39" },
                    { "Spain", "ES", "34" },
                    { "Belgium", "BE", "32" },
                    { "Greece", "GR", "30" },
                    { "Netherlands", "NL", "31" },
                    { "Portugal", "PT", "351" },
                    { "Sweden", "SE", "46" },
                    { "Switzerland", "CH", "41" },
                    { "Norway", "NO", "47" },
                    { "Denmark", "DK", "45" },
                    { "Poland", "PL", "48" },
                    { "Austria", "AT", "43" },
                    { "Romania", "RO", "40" },
                    { "Czech Republic", "CZ", "420" },
                    { "Ireland", "IE", "353" },
                    { "Russian Federation", "RU", "7" },
                    { "Hungary", "HU", "36" },
                    { "Ukraine", "UA", "380" },
                    { "Finland", "FI", "358" },
                    { "Bulgaria", "BG", "359" },
                    { "Croatia", "HR", "385" },
                    { "Serbia", "RS", "381" },
                    { "Slovakia", "SK", "421" },
                    { "Lithuania", "LT", "370" },
                    { "Slovenia", "SI", "386" },
                    { "Estonia", "EE", "372" },
                    { "Latvia", "LV", "371" },
                    { "Cyprus", "CY", "357" },
                    { "Luxembourg", "LU", "352" },
                    { "Malta", "MT", "356" },
                    { "Iceland", "IS", "354" },
                    { "Albania", "AL", "355" },
                    { "Moldova", "MD", "373"},
                    { "North Macedonia", "MK", "389" },
                    { "Montenegro", "ME", "382" },
                    { "Belarus", "BY", "375" },
                    { "Georgia", "GE", "995" },
                    { "Andorra", "AD", "376" },
                    { "Liechtenstein", "LI", "423" },
                    { "Monaco", "MC", "377" },
                    { "San Marino", "SM", "378" },
                    { "Vatican City", "VA", "379" },

                    //Middle Eastern countries
                    { "Saudi Arabia", "SA", "966" },
                    { "Egypt", "EG", "20" },
                    { "Turkey", "TR", "90" },
                    { "Israel", "IL", "972" },
                    { "Jordan", "JO", "962" },
                    { "Kuwait", "KW", "965" },
                    { "Oman", "OM", "968" },
                    { "United Arab Emirates", "AE", "971" },

                    //...more countries from Asia
                    { "Japan", "JP", "81"},
                    { "South Korea", "KR", "82" },
                    { "China", "CN", "86" },
                    { "India", "IN", "91"},
                    { "Indonesia", "ID", "62"},
                    { "Malaysia", "MY", "60"},
                    { "Philippines", "PH", "63"},
                    { "Singapore", "SG", "65"},
                    { "Thailand", "TH", "66"},
                    { "Vietnam", "VN", "84"},
                    { "Hong Kong", "HK", "852"},
                    { "Taiwan", "TW", "886"},
                    { "Bangladesh", "BD", "880"},
                    { "Sri Lanka", "LK", "94" },
                    { "Syria", "SY", "963" },
                    { "Lebanon", "LB", "961"},
                    { "Maldives", "MV", "960"},
                    { "Kazakhstan", "KZ", "7"},
                    { "Uzbekistan", "UZ", "998" },
                    { "Kyrgyzstan", "KG", "996" },
                    { "Turkmenistan", "TM", "993"},
                    { "Azerbaijan", "AZ", "994" },
                    { "Afghanistan", "AF", "93" },
                    { "Bahrain", "BH", "973"},
                    { "Brunei Darussalam", "BN", "673" },
                    { "Bhutan", "BT", "975"},
                    { "Cambodia", "KH", "855" },
                    { "East Timor", "TL", "670"},
                    { "Georgia", "GE", "995"},
                    { "Iran", "IR", "98" },
                    { "Korea", "KP", "850" },
                    { "Laos", "LA", "856" },
                    { "Macau", "MO", "853" },
                    { "Mongolia", "MN", "976" },
                    { "Myanmar", "MM", "95" },
                    { "Nepal", "NP", "977" },
                    // ... More countries from Africa
                    { "Algeria", "DZ", "213" },
                    { "Angola", "AO", "244"},
                    { "Benin", "BJ", "229"},
                    { "Botswana", "BW", "267" },
                    { "Burkina Faso", "BF", "226"},
                    { "Burundi", "BI", "257" },
                    { "Cameroon", "CM", "237" },
                    { "Cape Verde", "CV", "238" },
                    { "Central African Republic", "CF", "236" },
                    { "Chad", "TD", "235" },
                    { "Comoros", "KM", "269" },
                    { "Congo", "CG", "242" },
                    { "Djibouti", "DJ", "253"},
                    { "Equatorial Guinea", "GQ", "240" },
                    { "Eritrea", "ER", "291" },
                    { "Ethiopia", "ET", "251"},
                    { "Gabon", "GA", "241" },
                    { "Gambia", "GM", "220"},
                    { "Ghana", "GH", "233" },
                    { "Guinea", "GN", "224"},
                    { "Guinea-Bissau", "GW", "245"},
                    { "Ivory Coast", "CI", "225" },
                    { "Kenya", "KE", "254" },
                    { "Lesotho", "LS", "266" },
                    { "Liberia", "LR", "231" },
                    { "Libya", "LY", "218"},
                    { "Madagascar", "MG", "261"},
                    { "Malawi", "MW", "265"},
                    { "Mali", "ML", "223"},
                    { "Mauritania", "MR", "222"},
                    { "Mauritius", "MU", "230"},
                    { "Mayotte", "YT", "262" },
                    { "Morocco", "MA", "212"},
                    { "Mozambique", "MZ", "258" },
                    { "Namibia", "NA", "264"},
                    { "Niger", "NE", "227"},
                    { "Nigeria", "NG", "234"},
                    { "Rwanda", "RW", "250"},
                    { "Sao Tome and Principe", "ST", "239"},
                    { "Senegal", "SN", "221" },
                    { "Seychelles", "SC", "248" },
                    { "Sierra Leone", "SL", "232" },
                    { "Somalia", "SO", "252" },
                    { "South Africa", "ZA", "27" },
                    { "South Sudan", "SS", "211"},
                    { "Sudan", "SD", "249" },
                    { "Swaziland", "SZ", "268"},
                    { "Tanzania", "TZ", "255" },
                    { "Togo", "TG", "228" },
                    { "Tunisia", "TN", "216" },
                    { "Uganda", "UG", "256" },
                    { "Western Sahara", "EH", "212"},
                    { "Zambia", "ZM", "260"},
                    { "Zimbabwe", "ZW", "263"},

            });
            migrationBuilder.InsertData("languages", new[] { "name", "code" },
                new object[,] {
                { "English", "en" },
                { "Spanish", "es" },
                { "French", "fr" },
                { "German", "de" },
                { "Italian", "it" },
                { "Portuguese", "pt" },
                { "Russian", "ru" },
                { "Arabic", "ar" },
                { "Chinese", "zh" },
                { "Japanese", "ja" },
                { "Korean", "ko" },
                { "Turkish", "tr" },
                { "Dutch", "nl" },
                { "Polish", "pl" },
                { "Swedish", "sv" },
                { "Norwegian", "no" },
                { "Danish", "da" },
                { "Finnish", "fi" },
                { "Hebrew", "he" },
                { "Indonesian", "id" },
                { "Malay", "ms" },
                { "Thai", "th" },
                { "Vietnamese", "vi" },
                { "Hindi", "hi" },
                { "Bengali", "bn" },
                { "Punjabi", "pa" },
                { "Telugu", "te" },
                { "Tamil", "ta" },
                { "Marathi", "mr" },
                { "Gujarati", "gu" },
                { "Kannada", "kn" },
                { "Malayalam", "ml" },
                { "Odia", "or" },
                { "Assamese", "as" },
                { "Maithili", "bh" },
                { "Urdu", "ur" },
                { "Arabic", "ar" },
                { "Persian", "fa" },
                { "Amharic", "am" },
                { "Hausa", "ha" },
                { "Swahili", "sw" },
                { "Igbo", "ig" },
                { "Yoruba", "yo" },
                { "Oromo", "om" },
                });
            migrationBuilder.Sql($"UPDATE countries SET supported_at = now(), provider=1 WHERE country_code = 'IL'");
            migrationBuilder.Sql($"UPDATE languages SET supported_at = now() WHERE code in ('en', 'he')");
            //Adding Asian countries
            migrationBuilder.InsertData("AspNetRoles", new[] { "id", "name", "normalized_name" }, new object[,]
            {
                { Guid.NewGuid().ToString(), "Admin", "ADMIN" },
                { Guid.NewGuid().ToString(), "IT", "IT" },
                { Guid.NewGuid().ToString(), "Client", "CLIENT" }
            });

            var passwordHasher = new PasswordHasher<IdentityUser>();
            var user = new IdentityUser { UserName = "admin" };
            var passwordHash = passwordHasher.HashPassword(user, "ut!hf5353S");
            migrationBuilder.Sql(
                "INSERT INTO \"AspNetUsers\"  (id, user_name, normalized_user_name, email, normalized_email, email_confirmed, password_hash, security_stamp, concurrency_stamp, phone_number, phone_number_confirmed, two_factor_enabled, lockout_enabled, access_failed_count, token, first_name, last_name, country_id, display_language_id, address, register_completed_at, terms_approved) VALUES " +
                $"('{Guid.NewGuid().ToString()}', 'adminWolbergPro', 'ADMINWOLBERGPRO', 'admin@wolberg.pro', 'ADMIN@WOLBERG.PRO', true, '{passwordHash}','{Guid.NewGuid().ToString()}', '{Guid.NewGuid().ToString()}', '545566786', true, true, false, 0, '{StringExtensions.GenerateToken()}', 'Admin', 'User', (SELECT id FROM countries WHERE country_code = 'IL'), (SELECT id FROM languages WHERE code = 'en'), '', now(), true )");
            migrationBuilder.Sql($"INSERT INTO \"AspNetUserRoles\" (user_id, user_id1, role_id, role_id1) VALUES ((SELECT id FROM \"AspNetUsers\" WHERE user_name = 'adminWolbergPro'),(SELECT id FROM \"AspNetUsers\" WHERE user_name = 'adminWolbergPro'), (SELECT id FROM \"AspNetRoles\" WHERE name = 'Admin'), (SELECT id FROM \"AspNetRoles\" WHERE name = 'Admin'))");migrationBuilder.InsertData(
                "configurable-categories",
                new[] { "name", "description", "position" },
                new object[] { "Basic", "Basic", 1 });
            migrationBuilder.InsertData(
                "configurable-categories",
                new[] { "name", "description", "position" },
                new object[] { "Widget", "Widget Views", 2 });


            #region Setup Basic Filter Macros

            var sql = $"Insert into \"configurable-user-view-filter-macros\" (identifier, provider, description) values ";
            sql += $"('LoggedUser', 'LoggedUserMacro', 'Macro fetch logged user id'),";
            sql += $"('IpAddress', 'IpAddressMacro', 'Macro user Ip Address');";
            migrationBuilder.Sql(sql);

            #endregion

            #region Setup View

            sql = $"with userCte as ( SELECT id FROM \"AspNetUsers\" WHERE user_name='adminWolbergPro') \n";
            sql += $"insert into \"configurable-user-views\" (user_id, client_unique_key, \"name\", description, is_share_able, color, settings, permissions, is_predefined, entity_type) VALUES \n";
            sql += $"((SELECT id FROM userCte), '{Guid.NewGuid()}', 'Base View Of Users list', '', true, '#000000', '[]', '[]', true, '{EntityTypes.Users.ToString()}'), \n";
            sql += $"((SELECT id FROM userCte), '{Guid.NewGuid()}', 'Base View Of Client Contacts list', '', true, '#000000', '[]', '[]', true, '{EntityTypes.ClientContracts.ToString()}'), \n";
            sql += $"((SELECT id FROM userCte), '{Guid.NewGuid()}', 'Base View Of Clients list', '', true, '#000000', '[]', '[]', true, '{EntityTypes.Clients.ToString()}'), \n";
            sql += $"((SELECT id FROM userCte), '{Guid.NewGuid()}', 'Base View Of Activity Logs list', '', true, '#000000', '[]', '[]', true, '{EntityTypes.Activity.ToString()}'), \n";
            sql += $"((SELECT id FROM userCte), '{Guid.NewGuid()}', 'Base View Of User Activity Logs list', '', true, '#000000', '[]', '[]', true, '{EntityTypes.UserActivity.ToString()}'), \n";
            sql += $"((SELECT id FROM userCte), '{Guid.NewGuid()}', 'Base Dashboard Mission Widget View', '', false, '#000000', '[]', '[]', true, '{EntityTypes.MissionsWidgets.ToString()}');";
            migrationBuilder.Sql(sql);
            #endregion

            var metaData = new Dictionary<string, string>
            {
                { "width", "30%" }
            };
            string metaDataJson = JsonConvert.SerializeObject(metaData);

            #region activity_logs View

            sql = $"with categoryCte as ( SELECT id FROM \"configurable-categories\" WHERE name = 'Basic')";
            sql += $"INSERT INTO \"configurable-entity-column-definitions\" (entity_name, column_name, field_alias, field_path, display_name, description, category_id, permissions, meta_data, data_type, filter_operation_type, is_sortable, is_filterable, is_virtual_column) VALUES ";
            sql += $"('activity_logs', 'id', 'id', 'id', 'Activity Log Id', 'Entity Id', (SELECT id FROM categoryCte), '[]',  '{metaDataJson}', {(int)EntityColumnDataType.Number}, {(int)EntityColumnOperationType.IdFilterField}, true, true, false),";
            sql += $"('activity_logs', 'owner_user_id','ownerUserId', 'user.id', 'Owner User Id', 'Owner User Id', (SELECT id FROM categoryCte), '[\"Admin\",\"IT\"]', '{metaDataJson}', {(int)EntityColumnDataType.String}, {(int)EntityColumnOperationType.StringFilterField}, true, true, false),";
            sql += $"('activity_logs', 'owner_user_email','ownerUserEmail', 'user.email', 'Owner User Email', 'Owner User Email', (SELECT id FROM categoryCte), '[\"Admin\",\"IT\"]', '[]', {(int)EntityColumnDataType.String}, {(int)EntityColumnOperationType.StringFilterField}, true, true, true),";
            sql += $"('activity_logs', 'owner_user_first_name','ownerUserFirstName', 'user.firstName', 'Owner User First name', 'Owner User first name', (SELECT id FROM categoryCte), '[\"Admin\",\"IT\"]', '{metaDataJson}', {(int)EntityColumnDataType.String}, {(int)EntityColumnOperationType.StringFilterField}, true, true, true),";
            sql += $"('activity_logs', 'owner_user_last_name','ownerUserLastName', 'user.lastName', 'Owner User Last name', 'Owner User last name', (SELECT id FROM categoryCte), '[\"Admin\",\"IT\"]', '{metaDataJson}', {(int)EntityColumnDataType.String}, {(int)EntityColumnOperationType.StringFilterField}, true, true, true),";
            sql += $"('activity_logs', 'user_id', 'userId', 'user.id', 'User Id', 'User Id', (SELECT id FROM categoryCte), '[]', '{metaDataJson}', {(int)EntityColumnDataType.String}, {(int)EntityColumnOperationType.StringFilterField}, true, true, true),";
            sql += $"('activity_logs', 'user_email', 'userEmail', 'user.email', 'User Email', 'User Email', (SELECT id FROM categoryCte), '[]', '{metaDataJson}', {(int)EntityColumnDataType.String}, {(int)EntityColumnOperationType.StringFilterField}, true, true, true),";
            sql += $"('activity_logs', 'user_first_name', 'userFirstName', 'user.firstName', 'User First name', 'User first name', (SELECT id FROM categoryCte), '[]', '{metaDataJson}', {(int)EntityColumnDataType.String}, {(int)EntityColumnOperationType.StringFilterField}, true, true, true),";
            sql += $"('activity_logs', 'user_last_name', 'userLastName', 'user.lastName', 'User last name', 'User last name', (SELECT id FROM categoryCte), '[]', '{metaDataJson}', {(int)EntityColumnDataType.String}, {(int)EntityColumnOperationType.StringFilterField}, true, true, true),";
            sql += $"('activity_logs', 'entity_id', 'entityId', 'entityId', 'Entity Id', 'Entity Id', (SELECT id FROM categoryCte), '[]', '{metaDataJson}', {(int)EntityColumnDataType.String}, {(int)EntityColumnOperationType.StringFilterField}, true, true, false),";
            sql += $"('activity_logs', 'entity_type', 'entityType', 'entityType', 'Entity Type', 'Entity Type', (SELECT id FROM categoryCte), '[]', '{metaDataJson}', {(int)EntityColumnDataType.String}, {(int)EntityColumnOperationType.StringFilterField}, true, true, false),";
            sql += $"('activity_logs', 'operation_type', 'operationType', 'operationType', 'Operation Type', 'Operation Type', (SELECT id FROM categoryCte), '[]', '{metaDataJson}', {(int)EntityColumnDataType.String}, {(int)EntityColumnOperationType.StringFilterField}, true, true, false),";
            sql += $"('activity_logs', 'activity', 'activity', 'activity', 'Activity', 'Activity', (SELECT id FROM categoryCte), '[]', '{metaDataJson}', {(int)EntityColumnDataType.String}, {(int)EntityColumnOperationType.StringFilterField}, false, true, false),";
            sql += $"('activity_logs', 'details', 'details', 'details', 'Details', 'Details', (SELECT id FROM categoryCte), '[]', '{metaDataJson}', {(int)EntityColumnDataType.String}, {(int)EntityColumnOperationType.StringFilterField}, false, true, false),";
            sql += $"('activity_logs', 'status', 'status', 'status', 'Status', 'Status', (SELECT id FROM categoryCte), '[]', '{metaDataJson}', {(int)EntityColumnDataType.String}, {(int)EntityColumnOperationType.StringFilterField}, true, true, false),";
            sql += $"('activity_logs', 'ip_address', 'ipAddress', 'ipAddress', 'Ip Address', 'Ip Address', (SELECT id FROM categoryCte), '[]', '{metaDataJson}', {(int)EntityColumnDataType.String}, {(int)EntityColumnOperationType.StringFilterField}, true, true, false),";
            sql += $"('activity_logs', 'user_agent', 'userAgent', 'userAgent', 'User Agent', 'User Agent', (SELECT id FROM categoryCte), '[]', '{metaDataJson}', {(int)EntityColumnDataType.String}, {(int)EntityColumnOperationType.StringFilterField}, false, true, false),";
            sql += $"('activity_logs', 'created_at', 'createdAt', 'createdAt', 'Created At', 'Created At', (SELECT id FROM categoryCte), '[]', '{metaDataJson}', {(int)EntityColumnDataType.Date}, {(int)EntityColumnOperationType.DateFilterField}, true, true, false);";
            migrationBuilder.Sql(sql);

            sql = $"with viewCte as ( SELECT id FROM \"configurable-user-views\" WHERE entity_type = '{EntityTypes.Activity.ToString()}')";
            sql += $"INSERT INTO \"configurable-user-view-has-configurable-entity-column-definitions\" (configurable_user_view_id, configurable_entity_column_definition_id, is_hidden, is_fixed, position) VALUES ";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = 'activity_logs' and column_name = 'id'), false, false, 1),";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = 'activity_logs' and column_name = 'owner_user_id'), false, false, 2),";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = 'activity_logs' and column_name = 'entity_id'), false, false, 4),";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = 'activity_logs' and column_name = 'entity_type'), false, false, 5),";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = 'activity_logs' and column_name = 'operation_type'), false, false, 6),";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = 'activity_logs' and column_name = 'activity'), false, false, 7),";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = 'activity_logs' and column_name = 'details'), false, false, 8),";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = 'activity_logs' and column_name = 'status'), false, false, 9),";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = 'activity_logs' and column_name = 'ip_address'), false, false, 10),";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = 'activity_logs' and column_name = 'user_agent'), false, false, 11),";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = 'activity_logs' and column_name = 'created_at'), false, false, 12);";
            migrationBuilder.Sql(sql);

            #endregion

            #region User activity_logs View

            sql = $"with viewCte as (SELECT id FROM \"configurable-user-views\" WHERE entity_type = '{EntityTypes.UserActivity.ToString()}')";
            sql += $"INSERT INTO \"configurable-user-view-has-configurable-entity-column-definitions\" (configurable_user_view_id, configurable_entity_column_definition_id, is_hidden, is_fixed, position) VALUES ";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = 'activity_logs' and column_name = 'id'), false, false, 1),";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = 'activity_logs' and column_name = 'owner_user_id'), false, false, 2),";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = 'activity_logs' and column_name = 'user_id'), false, false, 3),";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = 'activity_logs' and column_name = 'entity_id'), false, false, 4),";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = 'activity_logs' and column_name = 'entity_type'), false, false, 5),";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = 'activity_logs' and column_name = 'operation_type'), false, false, 6),";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = 'activity_logs' and column_name = 'activity'), false, false, 7),";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = 'activity_logs' and column_name = 'details'), false, false, 8),";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = 'activity_logs' and column_name = 'status'), false, false, 9),";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = 'activity_logs' and column_name = 'ip_address'), false, false, 10),";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = 'activity_logs' and column_name = 'user_agent'), false, false, 11),";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = 'activity_logs' and column_name = 'created_at'), false, false, 12);";

            migrationBuilder.Sql(sql);
            
            #endregion

            #region Client View

            sql = $"with categoryCte as (SELECT id FROM \"configurable-categories\" WHERE name = 'Basic')";
            sql += $"INSERT INTO \"configurable-entity-column-definitions\" (entity_name, column_name, field_alias, field_path, display_name, field_formatter, description, category_id, permissions, meta_data, data_type, filter_operation_type, is_sortable, is_filterable, is_virtual_column) VALUES \n";
            sql += $"('clients', 'id', 'Id', 'id', 'Id', '', 'Id', (SELECT id FROM categoryCte), '[]',  '{metaDataJson}', {(int)EntityColumnDataType.Number}, {(int)EntityColumnOperationType.IdFilterField} , true, true, false), \n";
            sql += $"('clients', 'owner_user', 'OwnerUser', 'ownerUser.email', 'Owner User', '', 'User Owner of the clientId', (SELECT id FROM categoryCte), '[]',  '{metaDataJson}', {(int)EntityColumnDataType.String}, {(int)EntityColumnOperationType.StringFilterField} , true, true, true), \n";
            sql += $"('clients', 'name', 'Name', 'Name', 'name', '', 'Client Name', (SELECT id FROM categoryCte), '[]',  '{metaDataJson}', {(int)EntityColumnDataType.String}, {(int)EntityColumnOperationType.StringFilterField}, true, true, false), \n";
            sql += $"('clients', 'country_name', 'Client Country Name', 'country.countryName', 'Country Name', '', 'Client Country Name', (SELECT id FROM categoryCte), '[]',  '{metaDataJson}', {(int)EntityColumnDataType.Number}, {(int)EntityColumnOperationType.StringFilterField}, true, true, true), \n";
            sql += $"('clients', 'city', 'City', 'city', 'City', '', 'Client City', (SELECT id FROM categoryCte), '[]',  '{metaDataJson}', {(int)EntityColumnDataType.String}, {(int)EntityColumnOperationType.StringFilterField}, true, true, false), \n";
            sql += $"('clients', 'created_at', 'CreatedAt', 'createdAt', 'Register At', 'MM/DD/YYYY', 'Client Created At', (SELECT id FROM categoryCte), '[]',  '{metaDataJson}', {(int)EntityColumnDataType.Date}, {(int)EntityColumnOperationType.DateFilterField}, true, true, false);";

            migrationBuilder.Sql(sql);

            sql = $"with viewCte as (SELECT id FROM \"configurable-user-views\" WHERE entity_type = '{EntityTypes.Clients.ToString()}')";
            sql += $"INSERT INTO \"configurable-user-view-has-configurable-entity-column-definitions\" (configurable_user_view_id, configurable_entity_column_definition_id, is_hidden, is_fixed, position) VALUES ";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = 'clients' and column_name = 'id'), false, false, 1),";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = 'clients' and column_name = 'owner_user'), false, false, 3),";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = 'clients' and column_name = 'name'), false, false, 4),";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = 'clients' and column_name = 'country_name'), false, false, 5),";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = 'clients' and column_name = 'city'), false, false, 6),";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = 'clients' and column_name = 'created_at'), false, false, 7);";
            migrationBuilder.Sql(sql);

            #endregion

            #region client_contacts View

            sql = $"with categoryCte as (SELECT id FROM \"configurable-categories\" WHERE name = 'Basic')";
            sql += $"INSERT INTO \"configurable-entity-column-definitions\" (entity_name, column_name, field_alias, field_path, display_name, description, category_id, permissions, meta_data, data_type, filter_operation_type, is_sortable, is_filterable, is_virtual_column) VALUES ";
            sql += $"('client_contacts', 'id', 'Client Contact Id', 'id', 'id', 'Entity Id', (SELECT id FROM categoryCte), '[]',  '{metaDataJson}', {(int)EntityColumnDataType.Number}, {(int)EntityColumnOperationType.IdFilterField}, true, true, false),";
            sql += $"('client_contacts', 'client_id', 'Client Id', 'clientId', 'clientId', 'Client Id', (SELECT id FROM categoryCte), '[]',  '{metaDataJson}', {(int)EntityColumnDataType.Number}, {(int)EntityColumnOperationType.IdFilterField}, false, false, false),";
            sql += $"('client_contacts', 'first_name', 'Client Contact First Name', 'firstName', 'firstName', 'Client Contact First Name', (SELECT id FROM categoryCte), '[]',  '{metaDataJson}', {(int)EntityColumnDataType.String}, {(int)EntityColumnOperationType.StringFilterField}, true, true, false),";
            sql += $"('client_contacts', 'last_name', 'Client Contact Last Name', 'lastName', 'lastName', 'Client Contact Last Name', (SELECT id FROM categoryCte), '[]',  '{metaDataJson}', {(int)EntityColumnDataType.String}, {(int)EntityColumnOperationType.StringFilterField}, true, true, false),";
            sql += $"('client_contacts', 'email', 'Client Contact Email', 'email', 'email', 'Client Contact Email', (SELECT id FROM categoryCte), '[]',  '{metaDataJson}', {(int)EntityColumnDataType.String}, {(int)EntityColumnOperationType.StringFilterField}, true, true, false),";
            sql += $"('client_contacts', 'phone1', 'Client Contact Phone 1', 'phone1', 'phone1', 'Client Contact Phone 1', (SELECT id FROM categoryCte), '[]',  '{metaDataJson}', {(int)EntityColumnDataType.String}, {(int)EntityColumnOperationType.StringFilterField}, true, true, false),";
            sql += $"('client_contacts', 'phone2', 'Client Contact Phone 2', 'phone2', 'phone2', 'Client Contact Phone 2', (SELECT id FROM categoryCte), '[]',  '{metaDataJson}', {(int)EntityColumnDataType.String}, {(int)EntityColumnOperationType.StringFilterField}, true, true, false),";
            sql += $"('client_contacts', 'fax', 'Client Contact Fax', 'fax', 'fax', 'Client Contact Fax', (SELECT id FROM categoryCte), '[]',  '{metaDataJson}', {(int)EntityColumnDataType.String}, {(int)EntityColumnOperationType.StringFilterField}, false, false, false),";
            sql += $"('client_contacts', 'whatsapp', 'Client Contact Whatsapp', 'whatsapp', 'whatsapp', 'Client Contact Whatsapp', (SELECT id FROM categoryCte), '[]',  '{metaDataJson}', {(int)EntityColumnDataType.String}, {(int)EntityColumnOperationType.StringFilterField}, false, false, false),";
            sql += $"('client_contacts', 'country_id', 'Client Contact Country Id', 'countryId', 'country.id', 'Client Contact Country Id', (SELECT id FROM categoryCte), '[]',  '{metaDataJson}', {(int)EntityColumnDataType.Number}, {(int)EntityColumnOperationType.IdFilterField}, true, false, true),";
            sql += $"('client_contacts', 'country_name', 'Client Contact Country Name', 'countryName', 'country.countryName', 'Client Contact Country name', (SELECT id FROM categoryCte), '[]',  '{metaDataJson}', {(int)EntityColumnDataType.Number}, {(int)EntityColumnOperationType.IdFilterField}, true, false, true),";
            sql += $"('client_contacts', 'postal_code', 'Client Contact Postal Code', 'postalCode', 'postalCode', 'Client Contact Postal Code', (SELECT id FROM categoryCte), '[]',  '{metaDataJson}', {(int)EntityColumnDataType.String}, {(int)EntityColumnOperationType.StringFilterField}, false, false, false),";
            sql += $"('client_contacts', 'address', 'Client Contact Address', 'address', 'address', 'Client Contact Address', (SELECT id FROM categoryCte), '[]',  '{metaDataJson}', {(int)EntityColumnDataType.String}, {(int)EntityColumnOperationType.StringFilterField}, true, true, false),";
            sql += $"('client_contacts', 'city', 'Client Contact City', 'city', 'city', 'Client Contact City', (SELECT id FROM categoryCte), '[]',  '{metaDataJson}', {(int)EntityColumnDataType.String}, {(int)EntityColumnOperationType.StringFilterField}, true, true, false),";
            sql += $"('client_contacts', 'state', 'Client Contact State', 'state', 'state', 'Client Contact State', (SELECT id FROM categoryCte), '[]',  '{metaDataJson}', {(int)EntityColumnDataType.String}, {(int)EntityColumnOperationType.StringFilterField}, false, true, false),";
            sql += $"('client_contacts', 'job_title', 'Client Contact Job Title', 'jobTitle', 'jobTitle', 'Client Contact Job Title', (SELECT id FROM categoryCte), '[]',  '{metaDataJson}', {(int)EntityColumnDataType.String}, {(int)EntityColumnOperationType.StringFilterField}, false, true, false),";
            sql += $"('client_contacts', 'department', 'Client Contact Department', 'department', 'department', 'Client Contact Department', (SELECT id FROM categoryCte), '[]',  '{metaDataJson}', {(int)EntityColumnDataType.String}, {(int)EntityColumnOperationType.StringFilterField}, false, true, false),";
            sql += $"('client_contacts', 'company', 'Client Contact Company', 'company', 'company', 'Client Contact Company', (SELECT id FROM categoryCte), '[]',  '{metaDataJson}', {(int)EntityColumnDataType.String}, {(int)EntityColumnOperationType.StringFilterField}, true, false, false),";
            sql += $"('client_contacts', 'notes', 'Client Contact Notes', 'notes', 'notes', 'Client Contact Notes', (SELECT id FROM categoryCte), '[]',  '{metaDataJson}', {(int)EntityColumnDataType.String}, {(int)EntityColumnOperationType.StringFilterField}, false, false, false),";
            sql += $"('client_contacts', 'created_at', 'Client Contact Created At', 'createAt', 'createAt', 'Client Contact Created At', (SELECT id FROM categoryCte), '[]',  '{metaDataJson}', {(int)EntityColumnDataType.Date}, {(int)EntityColumnOperationType.DateFilterField}, true, true, false);";

            migrationBuilder.Sql(sql);

            sql = $"with viewCte as (SELECT id FROM \"configurable-user-views\" WHERE entity_type = '{EntityTypes.ClientContracts.ToString()}')";
            sql += $"INSERT INTO \"configurable-user-view-has-configurable-entity-column-definitions\" (configurable_user_view_id, configurable_entity_column_definition_id, is_hidden, is_fixed, position) VALUES ";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = 'client_contacts' and column_name = 'id'), false, false, 1),";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = 'client_contacts' and column_name = 'client_id'), false, false, 2),";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = 'client_contacts' and column_name = 'first_name'), false, false, 3),";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = 'client_contacts' and column_name = 'last_name'), false, false, 4),";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = 'client_contacts' and column_name = 'email'), false, false, 5),";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = 'client_contacts' and column_name = 'phone1'), false, false, 6),";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = 'client_contacts' and column_name = 'phone2'), false, false, 7),";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = 'client_contacts' and column_name = 'fax'), false, false, 8 ),";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = 'client_contacts' and column_name = 'whatsapp'), false, false, 9),";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = 'client_contacts' and column_name = 'country_id'), false, false, 10),";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = 'client_contacts' and column_name = 'postal_code'), false, false, 11),";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = 'client_contacts' and column_name = 'address'), false, false, 12),";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = 'client_contacts' and column_name = 'city'), false, false, 13),";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = 'client_contacts' and column_name = 'state'), false, false, 14),";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = 'client_contacts' and column_name = 'job_title'), false, false, 15),";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = 'client_contacts' and column_name = 'department'), false, false, 16),";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = 'client_contacts' and column_name = 'company'), false, false, 17),";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = 'client_contacts' and column_name = 'notes'), false, false, 18),";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = 'client_contacts' and column_name = 'created_at'), false, false, 19);";
            migrationBuilder.Sql(sql);

            #endregion

            #region Users list View

            sql = $"with categoryCte as (SELECT id FROM \"configurable-categories\" WHERE name = 'Basic')";
            sql += $"INSERT INTO \"configurable-entity-column-definitions\" (entity_name, column_name, field_alias, field_path, display_name, description, category_id, permissions, meta_data, data_type, filter_operation_type, is_sortable, is_filterable, is_virtual_column) VALUES ";
            sql += $"('application_user', 'id', 'id', 'id', 'User Id', 'User Id', (SELECT id FROM categoryCte), '[]',  '{metaDataJson}', {(int)EntityColumnDataType.String}, {(int)EntityColumnOperationType.GuidFilterField}, true, true, false),";
            sql += $"('application_user', 'email', 'email', 'email', 'User Email', 'User Email', (SELECT id FROM categoryCte), '[]',  '{metaDataJson}', {(int)EntityColumnDataType.String}, {(int)EntityColumnOperationType.StringFilterField}, true, true, false),";
            sql += $"('application_user', 'first_name', 'firstName', 'firstName', 'User First Name', 'User First Name', (SELECT id FROM categoryCte), '[]',  '{metaDataJson}', {(int)EntityColumnDataType.String}, {(int)EntityColumnOperationType.StringFilterField}, true, true, false),";
            sql += $"('application_user', 'last_name', 'lastName', 'lastName', 'User Last Name', 'User Last Name', (SELECT id FROM categoryCte), '[]',  '{metaDataJson}', {(int)EntityColumnDataType.String}, {(int)EntityColumnOperationType.StringFilterField}, true, true, false),";
            sql += $"('application_user', 'phone_number', 'phoneNumber', 'phoneNumber', 'User Phone Number', 'User Phone Number', (SELECT id FROM categoryCte), '[]',  '{metaDataJson}', {(int)EntityColumnDataType.String}, {(int)EntityColumnOperationType.StringFilterField}, true, true, false),";
            sql += $"('application_user', 'address', 'address', 'address', 'User Address', 'User Address', (SELECT id FROM categoryCte), '[]',  '{metaDataJson}', {(int)EntityColumnDataType.String}, {(int)EntityColumnOperationType.StringFilterField}, true, true, false),";
            sql += $"('application_user', 'country_id', 'country', 'country.id', 'User Country Id', 'User Country id', (SELECT id FROM categoryCte), '[]',  '{metaDataJson}', {(int)EntityColumnDataType.String}, {(int)EntityColumnOperationType.StringFilterField}, true, true, true),";
            sql += $"('application_user', 'country_name', 'country', 'country.countryName', 'User Country Name', 'User Country name', (SELECT id FROM categoryCte), '[]',  '{metaDataJson}', {(int)EntityColumnDataType.String}, {(int)EntityColumnOperationType.StringFilterField}, true, true, true),";
            sql += $"('application_user', 'display_language_id', 'displayLanguage', 'displayLanguage.id', 'User Display Language', 'User Display Language', (SELECT id FROM categoryCte), '[]',  '{metaDataJson}', {(int)EntityColumnDataType.String}, {(int)EntityColumnOperationType.StringFilterField}, true, true, true),";
            sql += $"('application_user', 'display_language_name', 'displayLanguage', 'displayLanguage.id', 'User Display Language', 'User Display Language', (SELECT id FROM categoryCte), '[]',  '{metaDataJson}', {(int)EntityColumnDataType.String}, {(int)EntityColumnOperationType.StringFilterField}, true, true, true),";
            sql += $"('application_user', 'roles', 'User Roles', 'roles', 'roles', 'User Roles', (SELECT id FROM categoryCte), '[]',  '{metaDataJson}', {(int)EntityColumnDataType.String}, {(int)EntityColumnOperationType.StringFilterField}, true, true, false);";

            migrationBuilder.Sql(sql);

            sql = $"with viewCte as (SELECT id FROM \"configurable-user-views\" WHERE entity_type = '{EntityTypes.Users.ToString()}')";
            sql += $"INSERT INTO \"configurable-user-view-has-configurable-entity-column-definitions\" (configurable_user_view_id, configurable_entity_column_definition_id, is_hidden, is_fixed, position) VALUES \n";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = 'application_user' and column_name = 'id'), false, false, 1), \n";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = 'application_user' and column_name = 'email'), false, false, 2), \n";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = 'application_user' and column_name = 'first_name'), false, false, 3), \n";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = 'application_user' and column_name = 'last_name'),false, false, 4), \n";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = 'application_user' and column_name = 'phone_number'), false, false, 5), \n";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = 'application_user' and column_name = 'address'), false, false, 6), \n";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = 'application_user' and column_name = 'country_id'), false, false, 7), \n";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = 'application_user' and column_name = 'country_name'), false, false, 8), \n";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = 'application_user' and column_name = 'display_language_id'), false, false, 9), \n";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = 'application_user' and column_name = 'display_language_name'),  false, false, 10), \n";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = 'application_user' and column_name = 'roles'), false, false, 11); \n";

            migrationBuilder.Sql(sql);
            #endregion

            #region Dashboard Widget Mission View

            sql = $"with categoryCte as (SELECT id FROM \"configurable-categories\" WHERE name = 'Widget')";
            sql += $"INSERT INTO \"configurable-entity-column-definitions\" (entity_name, column_name, field_alias, field_path, display_name, description, category_id, permissions, meta_data, data_type, filter_operation_type, is_sortable, is_filterable, is_virtual_column) VALUES ";
            sql += $"('{EntityTypes.MissionsWidgets.ToString()}', 'client', 'client', 'client', 'Client Name', 'Client Name', (SELECT id FROM categoryCte), '[]',  '{metaDataJson}', {(int)EntityColumnDataType.String}, {(int)EntityColumnOperationType.StringFilterField}, true, true, false),";
            sql += $"('{EntityTypes.MissionsWidgets.ToString()}', 'issue_by', 'issue_by', 'issueBy', 'Issue By', 'Issue By', (SELECT id FROM categoryCte), '[]',  '{metaDataJson}', {(int)EntityColumnDataType.String}, {(int)EntityColumnOperationType.StringFilterField}, true, true, false),";
            sql += $"('{EntityTypes.MissionsWidgets.ToString()}', 'title', 'title', 'title', 'Mission Title', 'Mission Title', (SELECT id FROM categoryCte), '[]',  '{metaDataJson}', {(int)EntityColumnDataType.String}, {(int)EntityColumnOperationType.StringFilterField}, true, true, false),";
            sql += $"('{EntityTypes.MissionsWidgets.ToString()}', 'profit', 'profit', 'profit', 'Total Profit', 'Total Profit from mission', (SELECT id FROM categoryCte), '[]',  '{metaDataJson}', {(int)EntityColumnDataType.Number}, {(int)EntityColumnOperationType.IntFilterField}, true, true, false),";
            sql += $"('{EntityTypes.MissionsWidgets.ToString()}', 'losses', 'losses', 'losses', 'Losess', 'Mission Total Losess', (SELECT id FROM categoryCte), '[]',  '{metaDataJson}', {(int)EntityColumnDataType.Number}, {(int)EntityColumnOperationType.IntFilterField}, true, true, false),";
            sql += $"('{EntityTypes.MissionsWidgets.ToString()}', 'status', 'status', 'status', 'Mission Status', 'Mission Status', (SELECT id FROM categoryCte), '[]',  '{metaDataJson}', {(int)EntityColumnDataType.String}, {(int)EntityColumnOperationType.StringFilterField}, true, true, false),";
            sql += $"('{EntityTypes.MissionsWidgets.ToString()}', 'report_by', 'report_by', 'reportBy', 'Report by', 'Report by', (SELECT id FROM categoryCte), '[]',  '{metaDataJson}', {(int)EntityColumnDataType.String}, {(int)EntityColumnOperationType.StringFilterField}, true, true, false),";
            sql += $"('{EntityTypes.MissionsWidgets.ToString()}', 'last_update', 'last_update', 'lastUpdate', 'Last Update', 'Last Update', (SELECT id FROM categoryCte), '[]',  '{metaDataJson}', {(int)EntityColumnDataType.Date}, {(int)EntityColumnOperationType.DateFilterField}, true, true, false);";

            migrationBuilder.Sql(sql);

            sql = $"with viewCte as (SELECT id FROM \"configurable-user-views\" WHERE entity_type = '{EntityTypes.MissionsWidgets.ToString()}')";
            sql += $"INSERT INTO \"configurable-user-view-has-configurable-entity-column-definitions\" (configurable_user_view_id, configurable_entity_column_definition_id, is_hidden, is_fixed, position) VALUES ";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = '{EntityTypes.MissionsWidgets.ToString()}' and column_name = 'issue_by'), false, false, 1),";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = '{EntityTypes.MissionsWidgets.ToString()}' and column_name = 'client'), false, false, 2),";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = '{EntityTypes.MissionsWidgets.ToString()}' and column_name = 'title'), false, false, 3),";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = '{EntityTypes.MissionsWidgets.ToString()}' and column_name = 'profit'), false, false, 4),";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = '{EntityTypes.MissionsWidgets.ToString()}' and column_name = 'losses'), false, false, 5),";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = '{EntityTypes.MissionsWidgets.ToString()}' and column_name = 'status'), false, false, 6),";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = '{EntityTypes.MissionsWidgets.ToString()}' and column_name = 'report_by'), false, false, 7),";
            sql += $"((SELECT id FROM viewCte), (SELECT id FROM \"configurable-entity-column-definitions\" WHERE entity_name = '{EntityTypes.MissionsWidgets.ToString()}' and column_name = 'last_update'), false, false, 8);";

            migrationBuilder.Sql(sql);
            #endregion
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
