CREATE TABLE [dbo].[Tg Bot Application Setup](
	[timestamp] [timestamp] NOT NULL,
	[Bot Id] [nvarchar](20) NOT NULL,
	[Bot Token] [nvarchar](50) NOT NULL,
	[API Url] [nvarchar](100) NOT NULL,
	[Telegram Files Download Path] [nvarchar](250) NOT NULL,
	[Service Files Network Path] [nvarchar](250) NULL,
	[User Authorization Attempts] [int] NOT NULL,
	[Send Files as Base64 API] [tinyint] NOT NULL,
 CONSTRAINT [Tg Application Setup$0] PRIMARY KEY CLUSTERED ([Bot Id] ASC) WITH (
	PAD_INDEX = OFF,
	STATISTICS_NORECOMPUTE = OFF,
	IGNORE_DUP_KEY = OFF,
	ALLOW_ROW_LOCKS = ON,
	ALLOW_PAGE_LOCKS = ON,
	OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY])
ON [PRIMARY]
GO;

CREATE TABLE [dbo].[Tg Authorization Attempts](
	[timestamp] [timestamp] NOT NULL,
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[User Id] [bigint] NOT NULL,
	[Chat Id] [bigint] NOT NULL,
	[Tg Username] [nvarchar](100) NOT NULL,
	[First Name] [nvarchar](100) NOT NULL,
	[Last Name] [nvarchar](100) NOT NULL,
	[Available Attempts Count] [int] NOT NULL,
 CONSTRAINT [Tg Authorization Attempts$0] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (
	PAD_INDEX = OFF,
	STATISTICS_NORECOMPUTE = OFF,
	IGNORE_DUP_KEY = OFF,
	ALLOW_ROW_LOCKS = ON,
	ALLOW_PAGE_LOCKS = ON,
	OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO;

CREATE UNIQUE NONCLUSTERED INDEX [$1] ON [dbo].[Tg Authorization Attempts]
(
	[User Id] ASC,
	[Chat Id] ASC,
	[Id] ASC) WITH (
		PAD_INDEX = OFF,
		STATISTICS_NORECOMPUTE = OFF,
		SORT_IN_TEMPDB = OFF,
		IGNORE_DUP_KEY = OFF,
		DROP_EXISTING = OFF,
		ONLINE = OFF,
		ALLOW_ROW_LOCKS = ON,
		ALLOW_PAGE_LOCKS = ON,
		OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO;

CREATE TABLE [dbo].[Tg User Sessions](
	[timestamp] [timestamp] NOT NULL,
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[User Id] [bigint] NOT NULL,
	[Chat Id] [bigint] NOT NULL,
	[Login] [nvarchar](50) NOT NULL,
	[Password Cipher] [nvarchar](150) NOT NULL,
	[Login Datetime] [datetime] NOT NULL,
	[Logout Datetime] [datetime] NOT NULL,
 CONSTRAINT [Tg User Sessions$0] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (
	PAD_INDEX = OFF,
	STATISTICS_NORECOMPUTE = OFF,
	IGNORE_DUP_KEY = OFF,
	ALLOW_ROW_LOCKS = ON,
	ALLOW_PAGE_LOCKS = ON,
	OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO;

CREATE UNIQUE NONCLUSTERED INDEX [$1] ON [dbo].[Tg User Sessions]
(
	[User Id] ASC,
	[Chat Id] ASC,
	[Logout Datetime] ASC,
	[Id] ASC) WITH (
		PAD_INDEX = OFF,
		STATISTICS_NORECOMPUTE = OFF,
		SORT_IN_TEMPDB = OFF,
		IGNORE_DUP_KEY = OFF,
		DROP_EXISTING = OFF,
		ONLINE = OFF,
		ALLOW_ROW_LOCKS = ON,
		ALLOW_PAGE_LOCKS = ON,
		OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO;

CREATE TABLE [dbo].[Tg Messages Ledger](
	[timestamp] [timestamp] NOT NULL,
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[User Id] [bigint] NOT NULL,
	[Chat Id] [bigint] NOT NULL,
	[Message Id] [int] NOT NULL,
	[Username] [nvarchar](100) NOT NULL,
	[Phone Number] [nvarchar](20) NOT NULL,
	[Location] [nvarchar](50) NOT NULL,
	[Message Type] [int] NOT NULL,
	[Message Content Type] [int] NOT NULL,
	[Message Text] [nvarchar](250) NOT NULL,
	[Created Datetime] [datetime] NOT NULL,
 CONSTRAINT [Tg Messages Ledger$0] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (
	PAD_INDEX = OFF,
	STATISTICS_NORECOMPUTE = OFF,
	IGNORE_DUP_KEY = OFF,
	ALLOW_ROW_LOCKS = ON,
	ALLOW_PAGE_LOCKS = ON,
	OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO;

CREATE TABLE [dbo].[Tg Web Requests Ledger](
	[timestamp] [timestamp] NOT NULL,
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[User Id] [bigint] NOT NULL,
	[Chat Id] [bigint] NOT NULL,
	[Message Id] [int] NOT NULL,
	[Web Request Endpoint] [nvarchar](250) NOT NULL,
	[Request Body] [nvarchar](250) NOT NULL,
	[Http Response Status Code] [int] NOT NULL,
	[Response Error Text] [nvarchar](250) NOT NULL,
	[Created Datetime] [datetime] NOT NULL,
 CONSTRAINT [Tg Web Requests Ledger$0] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (
	PAD_INDEX = OFF,
	STATISTICS_NORECOMPUTE = OFF,
	IGNORE_DUP_KEY = OFF,
	ALLOW_ROW_LOCKS = ON,
	ALLOW_PAGE_LOCKS = ON,
	OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO;