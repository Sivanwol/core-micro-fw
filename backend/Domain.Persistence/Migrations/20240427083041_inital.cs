using System;
using System.Collections.Generic;
using Domain.DTO.ConfigurableEntities;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Domain.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class inital : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // https://github.com/quartznet/quartznet/blob/main/database/tables/tables_postgres.sql
            migrationBuilder.Sql(@"set client_min_messages = WARNING;
            DROP TABLE IF EXISTS qrtz_fired_triggers;
            DROP TABLE IF EXISTS qrtz_paused_trigger_grps;
            DROP TABLE IF EXISTS qrtz_scheduler_state;
            DROP TABLE IF EXISTS qrtz_locks;
            DROP TABLE IF EXISTS qrtz_simprop_triggers;
            DROP TABLE IF EXISTS qrtz_simple_triggers;
            DROP TABLE IF EXISTS qrtz_cron_triggers;
            DROP TABLE IF EXISTS qrtz_blob_triggers;
            DROP TABLE IF EXISTS qrtz_triggers;
            DROP TABLE IF EXISTS qrtz_job_details;
            DROP TABLE IF EXISTS qrtz_calendars;
            set client_min_messages = NOTICE;
            
            CREATE TABLE qrtz_job_details
              (
                sched_name TEXT NOT NULL,
            	job_name  TEXT NOT NULL,
                job_group TEXT NOT NULL,
                description TEXT NULL,
                job_class_name   TEXT NOT NULL, 
                is_durable BOOL NOT NULL,
                is_nonconcurrent BOOL NOT NULL,
                is_update_data BOOL NOT NULL,
            	requests_recovery BOOL NOT NULL,
                job_data BYTEA NULL,
                PRIMARY KEY (sched_name,job_name,job_group)
            );
            
            CREATE TABLE qrtz_triggers
              (
                sched_name TEXT NOT NULL,
            	trigger_name TEXT NOT NULL,
                trigger_group TEXT NOT NULL,
                job_name  TEXT NOT NULL, 
                job_group TEXT NOT NULL,
                description TEXT NULL,
                next_fire_time BIGINT NULL,
                prev_fire_time BIGINT NULL,
                priority INTEGER NULL,
                trigger_state TEXT NOT NULL,
                trigger_type TEXT NOT NULL,
                start_time BIGINT NOT NULL,
                end_time BIGINT NULL,
                calendar_name TEXT NULL,
                misfire_instr SMALLINT NULL,
                job_data BYTEA NULL,
                PRIMARY KEY (sched_name,trigger_name,trigger_group),
                FOREIGN KEY (sched_name,job_name,job_group) 
            		REFERENCES qrtz_job_details(sched_name,job_name,job_group) 
            );
            
            CREATE TABLE qrtz_simple_triggers
              (
                sched_name TEXT NOT NULL,
            	trigger_name TEXT NOT NULL,
                trigger_group TEXT NOT NULL,
                repeat_count BIGINT NOT NULL,
                repeat_interval BIGINT NOT NULL,
                times_triggered BIGINT NOT NULL,
                PRIMARY KEY (sched_name,trigger_name,trigger_group),
                FOREIGN KEY (sched_name,trigger_name,trigger_group) 
            		REFERENCES qrtz_triggers(sched_name,trigger_name,trigger_group) ON DELETE CASCADE
            );
            
            CREATE TABLE QRTZ_SIMPROP_TRIGGERS 
              (
                sched_name TEXT NOT NULL,
                trigger_name TEXT NOT NULL ,
                trigger_group TEXT NOT NULL ,
                str_prop_1 TEXT NULL,
                str_prop_2 TEXT NULL,
                str_prop_3 TEXT NULL,
                int_prop_1 INTEGER NULL,
                int_prop_2 INTEGER NULL,
                long_prop_1 BIGINT NULL,
                long_prop_2 BIGINT NULL,
                dec_prop_1 NUMERIC NULL,
                dec_prop_2 NUMERIC NULL,
                bool_prop_1 BOOL NULL,
                bool_prop_2 BOOL NULL,
            	time_zone_id TEXT NULL,
            	PRIMARY KEY (sched_name,trigger_name,trigger_group),
                FOREIGN KEY (sched_name,trigger_name,trigger_group) 
            		REFERENCES qrtz_triggers(sched_name,trigger_name,trigger_group) ON DELETE CASCADE
            );
            
            CREATE TABLE qrtz_cron_triggers
              (
                sched_name TEXT NOT NULL,
                trigger_name TEXT NOT NULL,
                trigger_group TEXT NOT NULL,
                cron_expression TEXT NOT NULL,
                time_zone_id TEXT,
                PRIMARY KEY (sched_name,trigger_name,trigger_group),
                FOREIGN KEY (sched_name,trigger_name,trigger_group) 
            		REFERENCES qrtz_triggers(sched_name,trigger_name,trigger_group) ON DELETE CASCADE
            );
            
            CREATE TABLE qrtz_blob_triggers
              (
                sched_name TEXT NOT NULL,
                trigger_name TEXT NOT NULL,
                trigger_group TEXT NOT NULL,
                blob_data BYTEA NULL,
                PRIMARY KEY (sched_name,trigger_name,trigger_group),
                FOREIGN KEY (sched_name,trigger_name,trigger_group) 
            		REFERENCES qrtz_triggers(sched_name,trigger_name,trigger_group) ON DELETE CASCADE
            );
            
            CREATE TABLE qrtz_calendars
              (
                sched_name TEXT NOT NULL,
                calendar_name  TEXT NOT NULL, 
                calendar BYTEA NOT NULL,
                PRIMARY KEY (sched_name,calendar_name)
            );
            
            CREATE TABLE qrtz_paused_trigger_grps
              (
                sched_name TEXT NOT NULL,
                trigger_group TEXT NOT NULL, 
                PRIMARY KEY (sched_name,trigger_group)
            );
            
            CREATE TABLE qrtz_fired_triggers 
              (
                sched_name TEXT NOT NULL,
                entry_id TEXT NOT NULL,
                trigger_name TEXT NOT NULL,
                trigger_group TEXT NOT NULL,
                instance_name TEXT NOT NULL,
                fired_time BIGINT NOT NULL,
            	sched_time BIGINT NOT NULL,
                priority INTEGER NOT NULL,
                state TEXT NOT NULL,
                job_name TEXT NULL,
                job_group TEXT NULL,
                is_nonconcurrent BOOL NOT NULL,
                requests_recovery BOOL NULL,
                PRIMARY KEY (sched_name,entry_id)
            );
            
            CREATE TABLE qrtz_scheduler_state 
              (
                sched_name TEXT NOT NULL,
                instance_name TEXT NOT NULL,
                last_checkin_time BIGINT NOT NULL,
                checkin_interval BIGINT NOT NULL,
                PRIMARY KEY (sched_name,instance_name)
            );
            
            CREATE TABLE qrtz_locks
              (
                sched_name TEXT NOT NULL,
                lock_name  TEXT NOT NULL, 
                PRIMARY KEY (sched_name,lock_name)
            );
            
            create index idx_qrtz_j_req_recovery on qrtz_job_details(requests_recovery);
            create index idx_qrtz_t_next_fire_time on qrtz_triggers(next_fire_time);
            create index idx_qrtz_t_state on qrtz_triggers(trigger_state);
            create index idx_qrtz_t_nft_st on qrtz_triggers(next_fire_time,trigger_state);
            create index idx_qrtz_ft_trig_name on qrtz_fired_triggers(trigger_name);
            create index idx_qrtz_ft_trig_group on qrtz_fired_triggers(trigger_group);
            create index idx_qrtz_ft_trig_nm_gp on qrtz_fired_triggers(sched_name,trigger_name,trigger_group);
            create index idx_qrtz_ft_trig_inst_name on qrtz_fired_triggers(instance_name);
            create index idx_qrtz_ft_job_name on qrtz_fired_triggers(job_name);
            create index idx_qrtz_ft_job_group on qrtz_fired_triggers(job_group);
            create index idx_qrtz_ft_job_req_recovery on qrtz_fired_triggers(requests_recovery);");

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    concurrency_stamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "configurable-categories",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "varchar(100)", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    parent_category_id = table.Column<int>(type: "integer", nullable: true),
                    icon = table.Column<string>(type: "text", nullable: true),
                    position = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_configurable_categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "configurable-user-view-filter-macros",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    identifier = table.Column<string>(type: "text", nullable: false),
                    provider = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_configurable_user_view_filter_macros", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "countries",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    country_name = table.Column<string>(type: "varchar(50)", nullable: false),
                    country_code = table.Column<string>(type: "varchar(5)", nullable: false),
                    country_number = table.Column<string>(type: "varchar(5)", nullable: false),
                    supported_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    provider = table.Column<int>(type: "integer", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_countries", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "languages",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "varchar(20)", nullable: false),
                    code = table.Column<string>(type: "varchar(5)", nullable: false),
                    flag = table.Column<string>(type: "text", nullable: true),
                    supported_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_languages", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "media",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    file_name = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    path = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    mime_type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    size = table.Column<int>(type: "integer", nullable: false),
                    bucket_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_media", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tags",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tags", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role_id = table.Column<string>(type: "text", nullable: false),
                    claim_type = table.Column<string>(type: "text", nullable: true),
                    claim_value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_role_claims", x => x.id);
                    table.ForeignKey(
                        name: "fk_asp_net_role_claims_asp_net_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "AspNetRoles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "configurable-entity-column-definitions",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    entity_name = table.Column<string>(type: "varchar(100)", nullable: false),
                    column_name = table.Column<string>(type: "varchar(100)", nullable: true),
                    display_name = table.Column<string>(type: "varchar(100)", nullable: false),
                    field_formatter = table.Column<string>(type: "varchar(100)", nullable: true),
                    field_alias = table.Column<string>(type: "varchar(100)", nullable: false),
                    field_path = table.Column<string>(type: "varchar(500)", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    category_id = table.Column<int>(type: "integer", nullable: true),
                    data_type = table.Column<int>(type: "integer", nullable: false),
                    filter_operation_type = table.Column<int>(type: "integer", nullable: false),
                    is_sortable = table.Column<bool>(type: "boolean", nullable: false),
                    is_filterable = table.Column<bool>(type: "boolean", nullable: false),
                    disabled_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    permissions = table.Column<ICollection<string>>(type: "json", nullable: false),
                    is_virtual_column = table.Column<bool>(type: "boolean", nullable: false),
                    meta_data = table.Column<string>(type: "json", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_configurable_entity_column_definitions", x => x.id);
                    table.ForeignKey(
                        name: "fk_configurable_entity_column_definitions_configurable_categor",
                        column: x => x.category_id,
                        principalTable: "configurable-categories",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "contacts",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    first_name = table.Column<string>(type: "varchar(75)", nullable: false),
                    last_name = table.Column<string>(type: "varchar(75)", nullable: false),
                    email = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    phone1 = table.Column<string>(type: "varchar(20)", nullable: true),
                    phone2 = table.Column<string>(type: "varchar(20)", nullable: true),
                    fax = table.Column<string>(type: "varchar(20)", nullable: true),
                    whatsapp = table.Column<string>(type: "varchar(20)", nullable: true),
                    country_id = table.Column<int>(type: "integer", nullable: false),
                    postal_code = table.Column<string>(type: "varchar(10)", nullable: true),
                    address = table.Column<string>(type: "varchar(100)", nullable: true),
                    city = table.Column<string>(type: "varchar(100)", nullable: true),
                    state = table.Column<string>(type: "varchar(100)", nullable: true),
                    job_title = table.Column<string>(type: "varchar(100)", nullable: false),
                    department = table.Column<string>(type: "varchar(100)", nullable: true),
                    company = table.Column<string>(type: "varchar(100)", nullable: false),
                    notes = table.Column<string>(type: "text", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_contacts", x => x.id);
                    table.ForeignKey(
                        name: "fk_contacts_countries_country_id",
                        column: x => x.country_id,
                        principalTable: "countries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    country_id = table.Column<int>(type: "integer", nullable: false),
                    display_language_id = table.Column<int>(type: "integer", nullable: false),
                    token = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    first_name = table.Column<string>(type: "text", nullable: false),
                    last_name = table.Column<string>(type: "text", nullable: false),
                    address = table.Column<string>(type: "text", nullable: false),
                    terms_approved = table.Column<bool>(type: "boolean", nullable: false),
                    register_completed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    user_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_user_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    email_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: true),
                    security_stamp = table.Column<string>(type: "text", nullable: true),
                    concurrency_stamp = table.Column<string>(type: "text", nullable: true),
                    phone_number = table.Column<string>(type: "text", nullable: true),
                    phone_number_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    two_factor_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    lockout_end = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    lockout_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    access_failed_count = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_users", x => x.id);
                    table.ForeignKey(
                        name: "fk_asp_net_users_countries_country_id",
                        column: x => x.country_id,
                        principalTable: "countries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_asp_net_users_languages_display_language_id",
                        column: x => x.display_language_id,
                        principalTable: "languages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "providers",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    country_id = table.Column<int>(type: "integer", nullable: false),
                    logo_id = table.Column<int>(type: "integer", nullable: true),
                    site_url = table.Column<string>(type: "text", nullable: false),
                    support_url = table.Column<string>(type: "text", nullable: false),
                    city = table.Column<string>(type: "text", nullable: false),
                    address = table.Column<string>(type: "text", nullable: false),
                    support_phone = table.Column<string>(type: "text", nullable: true),
                    support_email = table.Column<string>(type: "text", nullable: true),
                    provider_type = table.Column<int>(type: "integer", nullable: false),
                    disabled_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_providers", x => x.id);
                    table.ForeignKey(
                        name: "fk_providers_countries_country_id",
                        column: x => x.country_id,
                        principalTable: "countries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_providers_media_logo_id",
                        column: x => x.logo_id,
                        principalTable: "media",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "activity-logs",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    owner_user_id = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: true),
                    user_id = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: true),
                    entity_id = table.Column<string>(type: "text", nullable: true),
                    entity_type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    operation_type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    activity = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    details = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ip_address = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    user_agent = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_activity_logs", x => x.id);
                    table.ForeignKey(
                        name: "fk_activity_logs_asp_net_users_owner_user_id",
                        column: x => x.owner_user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_activity_logs_asp_net_users_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<string>(type: "text", nullable: false),
                    claim_type = table.Column<string>(type: "text", nullable: true),
                    claim_value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_user_claims", x => x.id);
                    table.ForeignKey(
                        name: "fk_asp_net_user_claims_asp_net_users_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    login_provider = table.Column<string>(type: "text", nullable: false),
                    provider_key = table.Column<string>(type: "text", nullable: false),
                    provider_display_name = table.Column<string>(type: "text", nullable: true),
                    user_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_user_logins", x => new { x.login_provider, x.provider_key });
                    table.ForeignKey(
                        name: "fk_asp_net_user_logins_asp_net_users_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "text", nullable: false),
                    role_id = table.Column<string>(type: "text", nullable: false),
                    user_id1 = table.Column<string>(type: "text", nullable: false),
                    role_id1 = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_user_roles", x => new { x.user_id, x.role_id });
                    table.ForeignKey(
                        name: "fk_asp_net_user_roles_asp_net_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "AspNetRoles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_asp_net_user_roles_asp_net_roles_role_id1",
                        column: x => x.role_id1,
                        principalTable: "AspNetRoles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_asp_net_user_roles_asp_net_users_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_asp_net_user_roles_asp_net_users_user_id1",
                        column: x => x.user_id1,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "text", nullable: false),
                    login_provider = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_user_tokens", x => new { x.user_id, x.login_provider, x.name });
                    table.ForeignKey(
                        name: "fk_asp_net_user_tokens_asp_net_users_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "clients",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    owner_user_id = table.Column<string>(type: "text", nullable: false),
                    parent_id = table.Column<int>(type: "integer", nullable: true),
                    name = table.Column<string>(type: "varchar(100)", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    website = table.Column<string>(type: "varchar(500)", nullable: false),
                    disabled_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    country_id = table.Column<int>(type: "integer", nullable: false),
                    address = table.Column<string>(type: "varchar(100)", nullable: false),
                    city = table.Column<string>(type: "varchar(100)", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_clients", x => x.id);
                    table.ForeignKey(
                        name: "fk_clients_clients_parent_id",
                        column: x => x.parent_id,
                        principalTable: "clients",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_clients_countries_country_id",
                        column: x => x.country_id,
                        principalTable: "countries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_clients_users_owner_user_id",
                        column: x => x.owner_user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "configurable-user-views",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    parent_id = table.Column<int>(type: "integer", nullable: true),
                    client_unique_key = table.Column<Guid>(type: "uuid", nullable: false),
                    entity_type = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    is_share_able = table.Column<bool>(type: "boolean", nullable: false),
                    color = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: true),
                    settings = table.Column<ICollection<ConfigurableEntityMetaData>>(type: "json", nullable: false),
                    permissions = table.Column<ICollection<string>>(type: "json", nullable: false),
                    is_predefined = table.Column<bool>(type: "boolean", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_configurable_user_views", x => x.id);
                    table.ForeignKey(
                        name: "fk_configurable_user_views_configurable_user_views_parent_id",
                        column: x => x.parent_id,
                        principalTable: "configurable-user-views",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_configurable_user_views_users_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "configurable-user-view-tags",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_configurable_user_view_tags", x => x.id);
                    table.ForeignKey(
                        name: "fk_configurable_user_view_tags_users_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user-otps",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id1 = table.Column<string>(type: "text", nullable: true),
                    provider_type = table.Column<byte>(type: "smallint", nullable: false),
                    token = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    expiration_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    complate_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_otps", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_otps_users_user_id1",
                        column: x => x.user_id1,
                        principalTable: "AspNetUsers",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "user-preferences",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id1 = table.Column<string>(type: "text", nullable: true),
                    preference_key = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    preference_value = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_preferences", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_preferences_users_user_id1",
                        column: x => x.user_id1,
                        principalTable: "AspNetUsers",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "providers_has_categories",
                columns: table => new
                {
                    provider_id = table.Column<int>(type: "integer", nullable: false),
                    category_id = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_providers_has_categories", x => new { x.provider_id, x.category_id });
                    table.ForeignKey(
                        name: "fk_providers_has_categories_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_providers_has_categories_providers_provider_id",
                        column: x => x.provider_id,
                        principalTable: "providers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "providers_has_contacts",
                columns: table => new
                {
                    provider_id = table.Column<int>(type: "integer", nullable: false),
                    contact_id = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_providers_has_contacts", x => new { x.provider_id, x.contact_id });
                    table.ForeignKey(
                        name: "fk_providers_has_contacts_contacts_contact_id",
                        column: x => x.contact_id,
                        principalTable: "contacts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_providers_has_contacts_providers_provider_id",
                        column: x => x.provider_id,
                        principalTable: "providers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "client-contacts",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    client_id = table.Column<int>(type: "integer", nullable: false),
                    first_name = table.Column<string>(type: "varchar(75)", nullable: false),
                    last_name = table.Column<string>(type: "varchar(75)", nullable: false),
                    email = table.Column<string>(type: "varchar(500)", nullable: false),
                    phone1 = table.Column<string>(type: "varchar(20)", nullable: false),
                    phone2 = table.Column<string>(type: "varchar(20)", nullable: true),
                    fax = table.Column<string>(type: "varchar(20)", nullable: true),
                    whatsapp = table.Column<string>(type: "varchar(20)", nullable: true),
                    country_id = table.Column<int>(type: "integer", nullable: false),
                    postal_code = table.Column<string>(type: "varchar(10)", nullable: true),
                    address = table.Column<string>(type: "varchar(100)", nullable: true),
                    city = table.Column<string>(type: "varchar(100)", nullable: true),
                    state = table.Column<string>(type: "varchar(100)", nullable: true),
                    job_title = table.Column<string>(type: "varchar(100)", nullable: false),
                    department = table.Column<string>(type: "varchar(100)", nullable: true),
                    company = table.Column<string>(type: "varchar(100)", nullable: false),
                    notes = table.Column<string>(type: "text", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_client_contacts", x => x.id);
                    table.ForeignKey(
                        name: "fk_client_contacts_clients_client_id",
                        column: x => x.client_id,
                        principalTable: "clients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_client_contacts_countries_country_id",
                        column: x => x.country_id,
                        principalTable: "countries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "vendors",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    client_id = table.Column<int>(type: "integer", nullable: true),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    country_id = table.Column<int>(type: "integer", nullable: false),
                    logo_id = table.Column<int>(type: "integer", nullable: false),
                    media = table.Column<int>(type: "integer", nullable: true),
                    site_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    support_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    city = table.Column<string>(type: "text", nullable: false),
                    address = table.Column<string>(type: "text", nullable: false),
                    support_phone = table.Column<string>(type: "text", nullable: true),
                    support_email = table.Column<string>(type: "text", nullable: true),
                    support_response_type = table.Column<int>(type: "integer", nullable: false),
                    disabled_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_vendors", x => x.id);
                    table.ForeignKey(
                        name: "fk_vendors_clients_client_id",
                        column: x => x.client_id,
                        principalTable: "clients",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_vendors_countries_country_id",
                        column: x => x.country_id,
                        principalTable: "countries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_vendors_media_media",
                        column: x => x.media,
                        principalTable: "media",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "configurable-user-view-filters",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    view_id = table.Column<int>(type: "integer", nullable: false),
                    global = table.Column<bool>(type: "boolean", nullable: false),
                    filter_field_name = table.Column<string>(type: "text", nullable: false),
                    filter_date_type = table.Column<int>(type: "integer", nullable: false),
                    filter_field_type = table.Column<int>(type: "integer", nullable: false),
                    filter_collection_operation = table.Column<int>(type: "integer", nullable: false),
                    filter_operations = table.Column<int>(type: "integer", nullable: false),
                    filter_macro_value_id = table.Column<int>(type: "integer", nullable: true),
                    filter_values = table.Column<string>(type: "text", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_configurable_user_view_filters", x => x.id);
                    table.ForeignKey(
                        name: "fk_configurable_user_view_filters_configurable_user_views_view",
                        column: x => x.view_id,
                        principalTable: "configurable-user-views",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_configurable_user_view_filters_configurable_user_view_filte",
                        column: x => x.filter_macro_value_id,
                        principalTable: "configurable-user-view-filter-macros",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_configurable_user_view_filters_users_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "configurable-user-view-has-configurable-entity-column-definitions",
                columns: table => new
                {
                    configurable_user_view_id = table.Column<int>(type: "integer", nullable: false),
                    configurable_entity_column_definition_id = table.Column<int>(type: "integer", nullable: false),
                    is_hidden = table.Column<bool>(type: "boolean", nullable: false),
                    is_fixed = table.Column<bool>(type: "boolean", nullable: false),
                    position = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_configurable_user_view_has_configurable_entity_column_defin", x => new { x.configurable_user_view_id, x.configurable_entity_column_definition_id });
                    table.ForeignKey(
                        name: "fk_configurable_user_view_has_configurable_entity_column_defin",
                        column: x => x.configurable_entity_column_definition_id,
                        principalTable: "configurable-entity-column-definitions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_configurable_user_view_has_configurable_entity_column_defin1",
                        column: x => x.configurable_user_view_id,
                        principalTable: "configurable-user-views",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "configurable-user-view-has-configurable-user-view-tags",
                columns: table => new
                {
                    configurable_user_view_id = table.Column<int>(type: "integer", nullable: false),
                    configurable_user_view_tags_id = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_configurable_user_view_has_configurable_user_view_tags", x => new { x.configurable_user_view_id, x.configurable_user_view_tags_id });
                    table.ForeignKey(
                        name: "fk_configurable_user_view_has_configurable_user_view_tags_conf",
                        column: x => x.configurable_user_view_id,
                        principalTable: "configurable-user-views",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_configurable_user_view_has_configurable_user_view_tags_conf1",
                        column: x => x.configurable_user_view_tags_id,
                        principalTable: "configurable-user-view-tags",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "assets",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    client_id = table.Column<int>(type: "integer", nullable: false),
                    vendor_id = table.Column<int>(type: "integer", nullable: false),
                    provider_id = table.Column<int>(type: "integer", nullable: false),
                    label = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    notes = table.Column<string>(type: "text", nullable: false),
                    model = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    sub_model = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    service_tag = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    service_tag_media_id = table.Column<int>(type: "integer", nullable: false),
                    extra_media_id = table.Column<int>(type: "integer", nullable: false),
                    cpu = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    memory = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    storage_hdd1 = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    storage_hdd2 = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    storage_hdd3 = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    storage_hdd4 = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    storage_ssd1 = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    storage_ssd2 = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    storage_ssd3 = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    storage_ssd4 = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    type = table.Column<int>(type: "integer", nullable: false),
                    is_vm_supported = table.Column<bool>(type: "boolean", nullable: true),
                    is_raid_supported = table.Column<bool>(type: "boolean", nullable: true),
                    raid_number = table.Column<int>(type: "integer", nullable: true),
                    status = table.Column<int>(type: "integer", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_assets", x => x.id);
                    table.ForeignKey(
                        name: "fk_assets_clients_client_id",
                        column: x => x.client_id,
                        principalTable: "clients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_assets_media_extra_media_id",
                        column: x => x.extra_media_id,
                        principalTable: "media",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_assets_providers_provider_id",
                        column: x => x.provider_id,
                        principalTable: "providers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_assets_vendors_vendor_id",
                        column: x => x.vendor_id,
                        principalTable: "vendors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "vendor_has_contacts",
                columns: table => new
                {
                    vendor_id = table.Column<int>(type: "integer", nullable: false),
                    contact_id = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_vendor_has_contacts", x => new { x.vendor_id, x.contact_id });
                    table.ForeignKey(
                        name: "fk_vendor_has_contacts_contacts_contact_id",
                        column: x => x.contact_id,
                        principalTable: "contacts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_vendor_has_contacts_vendors_vendor_id",
                        column: x => x.vendor_id,
                        principalTable: "vendors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "vendor_metadata",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    vendor_id = table.Column<int>(type: "integer", nullable: false),
                    key = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<string>(type: "text", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_vendor_metadata", x => x.id);
                    table.ForeignKey(
                        name: "fk_vendor_metadata_vendors_vendor_id",
                        column: x => x.vendor_id,
                        principalTable: "vendors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "client-networks",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    client_id = table.Column<int>(type: "integer", nullable: false),
                    asset_id = table.Column<int>(type: "integer", nullable: false),
                    label = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    color = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    notes = table.Column<string>(type: "text", nullable: false),
                    disabled_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_client_networks", x => x.id);
                    table.ForeignKey(
                        name: "fk_client_networks_assets_asset_id",
                        column: x => x.asset_id,
                        principalTable: "assets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_client_networks_clients_client_id",
                        column: x => x.client_id,
                        principalTable: "clients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "client-hardware",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    client_id = table.Column<int>(type: "integer", nullable: false),
                    network_id = table.Column<int>(type: "integer", nullable: false),
                    asset_id = table.Column<int>(type: "integer", nullable: false),
                    label = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    notes = table.Column<string>(type: "text", nullable: false),
                    disabled_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_client_hardware", x => x.id);
                    table.ForeignKey(
                        name: "fk_client_hardware_assets_asset_id",
                        column: x => x.asset_id,
                        principalTable: "assets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_client_hardware_client_networks_network_id",
                        column: x => x.network_id,
                        principalTable: "client-networks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_client_hardware_clients_client_id",
                        column: x => x.client_id,
                        principalTable: "clients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "client-network-has-assets",
                columns: table => new
                {
                    client_network_id = table.Column<int>(type: "integer", nullable: false),
                    asset_id = table.Column<int>(type: "integer", nullable: false),
                    network_id = table.Column<int>(type: "integer", nullable: false),
                    label = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    notes = table.Column<string>(type: "text", nullable: false),
                    disabled_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_client_network_has_assets", x => new { x.client_network_id, x.asset_id });
                    table.ForeignKey(
                        name: "fk_client_network_has_assets_assets_asset_id",
                        column: x => x.asset_id,
                        principalTable: "assets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_client_network_has_assets_client_networks_network_id",
                        column: x => x.network_id,
                        principalTable: "client-networks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "client-network-has-tags",
                columns: table => new
                {
                    network_id = table.Column<int>(type: "integer", nullable: false),
                    tag_id = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_client_network_has_tags", x => new { x.network_id, x.tag_id });
                    table.ForeignKey(
                        name: "fk_client_network_has_tags_client_networks_network_id",
                        column: x => x.network_id,
                        principalTable: "client-networks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_client_network_has_tags_tags_tag_id",
                        column: x => x.tag_id,
                        principalTable: "tags",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "client-servers",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    client_id = table.Column<int>(type: "integer", nullable: false),
                    network_id = table.Column<int>(type: "integer", nullable: false),
                    label = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    notes = table.Column<string>(type: "text", nullable: false),
                    disabled_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_client_servers", x => x.id);
                    table.ForeignKey(
                        name: "fk_client_servers_client_networks_network_id",
                        column: x => x.network_id,
                        principalTable: "client-networks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_client_servers_clients_client_id",
                        column: x => x.client_id,
                        principalTable: "clients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "client-employees",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    client_id = table.Column<int>(type: "integer", nullable: false),
                    network_id = table.Column<int>(type: "integer", nullable: false),
                    server_id = table.Column<int>(type: "integer", nullable: false),
                    asset_id = table.Column<int>(type: "integer", nullable: false),
                    contact_employee_id = table.Column<int>(type: "integer", nullable: false),
                    disabled_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_client_employees", x => x.id);
                    table.ForeignKey(
                        name: "fk_client_employees_assets_asset_id",
                        column: x => x.asset_id,
                        principalTable: "assets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_client_employees_client_networks_network_id",
                        column: x => x.network_id,
                        principalTable: "client-networks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_client_employees_client_servers_server_id",
                        column: x => x.server_id,
                        principalTable: "client-servers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_client_employees_clients_client_id",
                        column: x => x.client_id,
                        principalTable: "clients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_client_employees_contacts_contact_employee_id",
                        column: x => x.contact_employee_id,
                        principalTable: "contacts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "client-server-has-assets",
                columns: table => new
                {
                    client_server_id = table.Column<int>(type: "integer", nullable: false),
                    asset_id = table.Column<int>(type: "integer", nullable: false),
                    server_id = table.Column<int>(type: "integer", nullable: false),
                    disabled_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    client_hardware_id = table.Column<int>(type: "integer", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_client_server_has_assets", x => new { x.client_server_id, x.asset_id });
                    table.ForeignKey(
                        name: "fk_client_server_has_assets_assets_asset_id",
                        column: x => x.asset_id,
                        principalTable: "assets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_client_server_has_assets_client_hardware_client_hardware_id",
                        column: x => x.client_hardware_id,
                        principalTable: "client-hardware",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_client_server_has_assets_client_servers_server_id",
                        column: x => x.server_id,
                        principalTable: "client-servers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "client-server-has-tags",
                columns: table => new
                {
                    server_id = table.Column<int>(type: "integer", nullable: false),
                    tag_id = table.Column<int>(type: "integer", nullable: false),
                    client_hardware_id = table.Column<int>(type: "integer", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_client_server_has_tags", x => new { x.server_id, x.tag_id });
                    table.ForeignKey(
                        name: "fk_client_server_has_tags_client_hardware_client_hardware_id",
                        column: x => x.client_hardware_id,
                        principalTable: "client-hardware",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_client_server_has_tags_client_servers_server_id",
                        column: x => x.server_id,
                        principalTable: "client-servers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_client_server_has_tags_tags_tag_id",
                        column: x => x.tag_id,
                        principalTable: "tags",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "client-employees-has-assets",
                columns: table => new
                {
                    client_employee_id = table.Column<int>(type: "integer", nullable: false),
                    asset_id = table.Column<int>(type: "integer", nullable: false),
                    employee_id = table.Column<int>(type: "integer", nullable: false),
                    contact_employee_id = table.Column<int>(type: "integer", nullable: false),
                    disabled_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_client_employees_has_assets", x => new { x.client_employee_id, x.asset_id });
                    table.ForeignKey(
                        name: "fk_client_employees_has_assets_assets_asset_id",
                        column: x => x.asset_id,
                        principalTable: "assets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_client_employees_has_assets_client_contacts_contact_employe",
                        column: x => x.contact_employee_id,
                        principalTable: "client-contacts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_client_employees_has_assets_client_employees_employee_id",
                        column: x => x.employee_id,
                        principalTable: "client-employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "client-employees-has-tags",
                columns: table => new
                {
                    employee_id = table.Column<int>(type: "integer", nullable: false),
                    tag_id = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_client_employees_has_tags", x => new { x.employee_id, x.tag_id });
                    table.ForeignKey(
                        name: "fk_client_employees_has_tags_client_employees_employee_id",
                        column: x => x.employee_id,
                        principalTable: "client-employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_client_employees_has_tags_tags_tag_id",
                        column: x => x.tag_id,
                        principalTable: "tags",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_activity_logs_owner_user_id",
                table: "activity-logs",
                column: "owner_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_activity_logs_user_id",
                table: "activity-logs",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_asp_net_role_claims_role_id",
                table: "AspNetRoleClaims",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "normalized_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_asp_net_user_claims_user_id",
                table: "AspNetUserClaims",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_asp_net_user_logins_user_id",
                table: "AspNetUserLogins",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_asp_net_user_roles_role_id",
                table: "AspNetUserRoles",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "ix_asp_net_user_roles_role_id1",
                table: "AspNetUserRoles",
                column: "role_id1");

            migrationBuilder.CreateIndex(
                name: "ix_asp_net_user_roles_user_id1",
                table: "AspNetUserRoles",
                column: "user_id1");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "normalized_email");

            migrationBuilder.CreateIndex(
                name: "ix_asp_net_users_country_id",
                table: "AspNetUsers",
                column: "country_id");

            migrationBuilder.CreateIndex(
                name: "ix_asp_net_users_display_language_id",
                table: "AspNetUsers",
                column: "display_language_id");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "normalized_user_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_assets_client_id",
                table: "assets",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_assets_extra_media_id",
                table: "assets",
                column: "extra_media_id");

            migrationBuilder.CreateIndex(
                name: "ix_assets_provider_id",
                table: "assets",
                column: "provider_id");

            migrationBuilder.CreateIndex(
                name: "ix_assets_vendor_id",
                table: "assets",
                column: "vendor_id");

            migrationBuilder.CreateIndex(
                name: "ix_client_contacts_client_id",
                table: "client-contacts",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_client_contacts_country_id_first_name_last_name",
                table: "client-contacts",
                columns: new[] { "country_id", "first_name", "last_name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_client_contacts_email",
                table: "client-contacts",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_client_contacts_first_name_last_name_client_id_country_id",
                table: "client-contacts",
                columns: new[] { "first_name", "last_name", "client_id", "country_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_client_employees_asset_id",
                table: "client-employees",
                column: "asset_id");

            migrationBuilder.CreateIndex(
                name: "ix_client_employees_client_id",
                table: "client-employees",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_client_employees_contact_employee_id",
                table: "client-employees",
                column: "contact_employee_id");

            migrationBuilder.CreateIndex(
                name: "ix_client_employees_network_id",
                table: "client-employees",
                column: "network_id");

            migrationBuilder.CreateIndex(
                name: "ix_client_employees_server_id",
                table: "client-employees",
                column: "server_id");

            migrationBuilder.CreateIndex(
                name: "ix_client_employees_has_assets_asset_id",
                table: "client-employees-has-assets",
                column: "asset_id");

            migrationBuilder.CreateIndex(
                name: "ix_client_employees_has_assets_contact_employee_id",
                table: "client-employees-has-assets",
                column: "contact_employee_id");

            migrationBuilder.CreateIndex(
                name: "ix_client_employees_has_assets_employee_id",
                table: "client-employees-has-assets",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "ix_client_employees_has_tags_tag_id",
                table: "client-employees-has-tags",
                column: "tag_id");

            migrationBuilder.CreateIndex(
                name: "ix_client_hardware_asset_id",
                table: "client-hardware",
                column: "asset_id");

            migrationBuilder.CreateIndex(
                name: "ix_client_hardware_client_id",
                table: "client-hardware",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_client_hardware_network_id",
                table: "client-hardware",
                column: "network_id");

            migrationBuilder.CreateIndex(
                name: "ix_client_network_has_assets_asset_id",
                table: "client-network-has-assets",
                column: "asset_id");

            migrationBuilder.CreateIndex(
                name: "ix_client_network_has_assets_network_id",
                table: "client-network-has-assets",
                column: "network_id");

            migrationBuilder.CreateIndex(
                name: "ix_client_network_has_tags_tag_id",
                table: "client-network-has-tags",
                column: "tag_id");

            migrationBuilder.CreateIndex(
                name: "ix_client_networks_asset_id",
                table: "client-networks",
                column: "asset_id");

            migrationBuilder.CreateIndex(
                name: "ix_client_networks_client_id",
                table: "client-networks",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_client_server_has_assets_asset_id",
                table: "client-server-has-assets",
                column: "asset_id");

            migrationBuilder.CreateIndex(
                name: "ix_client_server_has_assets_client_hardware_id",
                table: "client-server-has-assets",
                column: "client_hardware_id");

            migrationBuilder.CreateIndex(
                name: "ix_client_server_has_assets_server_id",
                table: "client-server-has-assets",
                column: "server_id");

            migrationBuilder.CreateIndex(
                name: "ix_client_server_has_tags_client_hardware_id",
                table: "client-server-has-tags",
                column: "client_hardware_id");

            migrationBuilder.CreateIndex(
                name: "ix_client_server_has_tags_tag_id",
                table: "client-server-has-tags",
                column: "tag_id");

            migrationBuilder.CreateIndex(
                name: "ix_client_servers_client_id",
                table: "client-servers",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_client_servers_network_id",
                table: "client-servers",
                column: "network_id");

            migrationBuilder.CreateIndex(
                name: "ix_clients_country_id",
                table: "clients",
                column: "country_id");

            migrationBuilder.CreateIndex(
                name: "ix_clients_name_owner_user_id",
                table: "clients",
                columns: new[] { "name", "owner_user_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_clients_owner_user_id_parent_id_country_id_address_city_name",
                table: "clients",
                columns: new[] { "owner_user_id", "parent_id", "country_id", "address", "city", "name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_clients_parent_id",
                table: "clients",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "ix_configurable_entity_column_definitions_category_id",
                table: "configurable-entity-column-definitions",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "ix_configurable_entity_column_definitions_entity_name_column_n",
                table: "configurable-entity-column-definitions",
                columns: new[] { "entity_name", "column_name", "display_name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_configurable_user_view_filters_filter_macro_value_id",
                table: "configurable-user-view-filters",
                column: "filter_macro_value_id");

            migrationBuilder.CreateIndex(
                name: "ix_configurable_user_view_filters_user_id_view_id",
                table: "configurable-user-view-filters",
                columns: new[] { "user_id", "view_id" });

            migrationBuilder.CreateIndex(
                name: "ix_configurable_user_view_filters_view_id",
                table: "configurable-user-view-filters",
                column: "view_id");

            migrationBuilder.CreateIndex(
                name: "ix_configurable_user_view_has_configurable_entity_column_defin",
                table: "configurable-user-view-has-configurable-entity-column-definitions",
                column: "configurable_entity_column_definition_id");

            migrationBuilder.CreateIndex(
                name: "ix_configurable_user_view_has_configurable_user_view_tags_conf",
                table: "configurable-user-view-has-configurable-user-view-tags",
                column: "configurable_user_view_tags_id");

            migrationBuilder.CreateIndex(
                name: "ix_configurable_user_views_parent_id",
                table: "configurable-user-views",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "ix_configurable_user_views_user_id_name",
                table: "configurable-user-views",
                columns: new[] { "user_id", "name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_configurable_user_view_tags_user_id_name",
                table: "configurable-user-view-tags",
                columns: new[] { "user_id", "name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_contacts_country_id_first_name_last_name",
                table: "contacts",
                columns: new[] { "country_id", "first_name", "last_name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_contacts_email",
                table: "contacts",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_providers_country_id",
                table: "providers",
                column: "country_id");

            migrationBuilder.CreateIndex(
                name: "ix_providers_logo_id",
                table: "providers",
                column: "logo_id");

            migrationBuilder.CreateIndex(
                name: "ix_providers_name",
                table: "providers",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_providers_has_categories_category_id",
                table: "providers_has_categories",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "ix_providers_has_contacts_contact_id",
                table: "providers_has_contacts",
                column: "contact_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_otps_token",
                table: "user-otps",
                column: "token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_otps_user_id1",
                table: "user-otps",
                column: "user_id1");

            migrationBuilder.CreateIndex(
                name: "ix_user_preferences_user_id_preference_key",
                table: "user-preferences",
                columns: new[] { "UserId", "preference_key" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_preferences_user_id1",
                table: "user-preferences",
                column: "user_id1");

            migrationBuilder.CreateIndex(
                name: "ix_vendor_has_contacts_contact_id",
                table: "vendor_has_contacts",
                column: "contact_id");

            migrationBuilder.CreateIndex(
                name: "ix_vendor_metadata_vendor_id",
                table: "vendor_metadata",
                column: "vendor_id");

            migrationBuilder.CreateIndex(
                name: "ix_vendors_client_id",
                table: "vendors",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "ix_vendors_country_id",
                table: "vendors",
                column: "country_id");

            migrationBuilder.CreateIndex(
                name: "ix_vendors_media",
                table: "vendors",
                column: "media");

            migrationBuilder.CreateIndex(
                name: "ix_vendors_name",
                table: "vendors",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "activity-logs");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "client-employees-has-assets");

            migrationBuilder.DropTable(
                name: "client-employees-has-tags");

            migrationBuilder.DropTable(
                name: "client-network-has-assets");

            migrationBuilder.DropTable(
                name: "client-network-has-tags");

            migrationBuilder.DropTable(
                name: "client-server-has-assets");

            migrationBuilder.DropTable(
                name: "client-server-has-tags");

            migrationBuilder.DropTable(
                name: "configurable-user-view-filters");

            migrationBuilder.DropTable(
                name: "configurable-user-view-has-configurable-entity-column-definitions");

            migrationBuilder.DropTable(
                name: "configurable-user-view-has-configurable-user-view-tags");

            migrationBuilder.DropTable(
                name: "providers_has_categories");

            migrationBuilder.DropTable(
                name: "providers_has_contacts");

            migrationBuilder.DropTable(
                name: "user-otps");

            migrationBuilder.DropTable(
                name: "user-preferences");

            migrationBuilder.DropTable(
                name: "vendor_has_contacts");

            migrationBuilder.DropTable(
                name: "vendor_metadata");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "client-contacts");

            migrationBuilder.DropTable(
                name: "client-employees");

            migrationBuilder.DropTable(
                name: "client-hardware");

            migrationBuilder.DropTable(
                name: "tags");

            migrationBuilder.DropTable(
                name: "configurable-user-view-filter-macros");

            migrationBuilder.DropTable(
                name: "configurable-entity-column-definitions");

            migrationBuilder.DropTable(
                name: "configurable-user-views");

            migrationBuilder.DropTable(
                name: "configurable-user-view-tags");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "client-servers");

            migrationBuilder.DropTable(
                name: "contacts");

            migrationBuilder.DropTable(
                name: "configurable-categories");

            migrationBuilder.DropTable(
                name: "client-networks");

            migrationBuilder.DropTable(
                name: "assets");

            migrationBuilder.DropTable(
                name: "providers");

            migrationBuilder.DropTable(
                name: "vendors");

            migrationBuilder.DropTable(
                name: "clients");

            migrationBuilder.DropTable(
                name: "media");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "countries");

            migrationBuilder.DropTable(
                name: "languages");
        }
    }
}
