CREATE TABLE [dbo].[Telegram Users](
	[timestamp] [timestamp] NOT NULL,
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Login] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[Is Active] [tinyint] NOT NULL,
	[Is Admin] [tinyint] NOT NULL,
 CONSTRAINT [Telegram Users$0] PRIMARY KEY CLUSTERED ([Id] ASC)
 WITH (PAD_INDEX = OFF,
       STATISTICS_NORECOMPUTE = OFF,
	   IGNORE_DUP_KEY = OFF,
	   ALLOW_ROW_LOCKS = ON,
	   ALLOW_PAGE_LOCKS = ON,
	   OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
 ON [PRIMARY]
) ON [PRIMARY];