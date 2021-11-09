CREATE TABLE [dbo].[Tbl_Ads] (
    [Ad_Id]        INT          IDENTITY (1, 1) NOT NULL,
    [Ad_Headline]   VARCHAR (30) NOT NULL,
    [Ad_Content]   VARCHAR (200) NOT NULL,
    [Ad_ArticlePrice]   INT NOT NULL,
    [Ad_AdPriceId]   INT NOT NULL,
    [Ad_AdTypeId]   INT NOT NULL,
    CONSTRAINT [PK_Tbl_Ads] PRIMARY KEY CLUSTERED ([Ad_Id] ASC)
    CONSTRAINT [FK1_Tbl_Ads] FOREIGN KEY ([Ad_AdPriceId]) REFERENCES [dbo].[Tbl_Prices] ([Price_Id])
    CONSTRAINT [FK2_Tbl_Ads] FOREIGN KEY ([Ad_AdTypeId]) REFERENCES [dbo].[Tbl_AdType] ([AdType_Id]),
);
GO


CREATE TABLE [dbo].[Tbl_Prices] (
    [Price_Id]        INT          IDENTITY (1, 1) NOT NULL,
    [Price_AdPrice]   INT NOT NULL,
    CONSTRAINT [PK_Tbl_Prices] PRIMARY KEY CLUSTERED ([Price_Id] ASC),
    CONSTRAINT [UQ_Tbl_Prices] UNIQUE NONCLUSTERED ([Price_AdPrice]));
GO


INSERT INTO[dbo].[Tbl_Prices] 
([Price_AdPrice])
Values 
(0),
(40) 
GO


CREATE TABLE [dbo].[Tbl_AdType] (
    [AdType_Id]        INT          IDENTITY (1, 1) NOT NULL,
    [AdType_Text]   VARCHAR (20) NOT NULL,
    CONSTRAINT [PK_Tbl_AdType] PRIMARY KEY CLUSTERED ([AdType_Id] ASC),
    CONSTRAINT [UQ_Tbl_AdType] UNIQUE NONCLUSTERED ([AdType_Text]));
GO


INSERT INTO[dbo].[Tbl_AdType] 
([AdType_Text])
Values 
('Privatperson'),
('Företag') 
GO


CREATE TABLE [dbo].[Tbl_Advertisers] (
    [Advertiser_Id]  INT NOT NULL,
    [Advertiser_AddressId]   INT NOT NULL,
    [Advertiser_BillingId]   INT,
    [Advertiser_AdId]   INT NOT NULL
    CONSTRAINT [FK_Tbl_Advertisers] FOREIGN KEY ([Advertiser_AdId]) REFERENCES [dbo].[Tbl_Ads] ([Ad_Id])
    ON DELETE CASCADE
    ON UPDATE CASCADE
);
GO


ALTER TABLE [Tbl_Advertisers]
ADD CONSTRAINT [UQ_Tbl_Advertisers] UNIQUE NONCLUSTERED ([Advertiser_Id] ASC, [Advertiser_AdId] ASC)
GO


CREATE OR ALTER PROC [dbo].[GetAllAds]
AS
BEGIN
    SELECT Tbl_Ads.Ad_Id, Tbl_Ads.Ad_Headline, Tbl_Ads.Ad_Content,
    Tbl_Ads.Ad_ArticlePrice, Tbl_Prices.Price_AdPrice, Tbl_AdType.AdType_Id, Tbl_AdType.AdType_Text,
    Tbl_Advertisers.Advertiser_Id, Tbl_Advertisers.Advertiser_AddressId,
    Tbl_Advertisers.Advertiser_BillingId   
    FROM
    Tbl_Ads
    JOIN Tbl_Advertisers ON Ad_Id = Tbl_Advertisers.Advertiser_AdId
    JOIN Tbl_Prices ON Ad_AdPriceId = Tbl_Prices.Price_Id
    JOIN Tbl_AdType ON Ad_AdTypeId = Tbl_AdType.AdType_Id
END
GO



CREATE OR ALTER PROC [dbo].[GetAd]
@Ad_Id INT
AS
BEGIN
    SELECT Tbl_Ads.Ad_Id, Tbl_Ads.Ad_Headline, Tbl_Ads.Ad_Content,
    Tbl_Ads.Ad_ArticlePrice, Tbl_Prices.Price_AdPrice, Tbl_AdType.AdType_Id, Tbl_AdType.AdType_Text,
    Tbl_Advertisers.Advertiser_Id, Tbl_Advertisers.Advertiser_AddressId,
    Tbl_Advertisers.Advertiser_BillingId   
    FROM
    Tbl_Ads
    JOIN Tbl_Advertisers ON Ad_Id = Tbl_Advertisers.Advertiser_AdId
    JOIN Tbl_Prices ON Ad_AdPriceId = Tbl_Prices.Price_Id
    JOIN Tbl_AdType ON Ad_AdTypeId = Tbl_AdType.AdType_Id
    WHERE Tbl_Ads.Ad_Id = @Ad_Id;
END
GO



CREATE OR ALTER PROC [dbo].[CreateAdd] 
    @Headline VARCHAR(30),
    @Content VARCHAR(200),
    @ArticlePrice INT,
    @AdPriceId INT,
    @AdTypeId INT,
    @AdvertiserId INT,
    @AdvertiserAddressId INT, 
    @AdvertiserBillingId INT = NULL
AS
BEGIN
    DECLARE @LastId TABLE (Ad_Id INT)
    DECLARE @Ad_Id INT

    INSERT INTO[dbo].[Tbl_Ads]
    ([Ad_Headline], [Ad_Content], [Ad_ArticlePrice], [Ad_AdPriceId], [Ad_AdTypeId])
    OUTPUT INSERTED.Ad_Id INTO @LastId
    VALUES 
    (@Headline, @Content, @ArticlePrice, @AdPriceId, @AdTypeId)

    SET @Ad_Id = (SELECT Ad_Id FROM @LastId)

    INSERT INTO[dbo].[Tbl_Advertisers] 
    ([Advertiser_Id], [Advertiser_AddressId], [Advertiser_BillingId], [Advertiser_AdId])
    VALUES 
    (@AdvertiserId, @AdvertiserAddressId, @AdvertiserBillingId, @Ad_Id)
    SELECT Ad_Id FROM Tbl_Ads WHERE Ad_Id = @Ad_Id;
END
GO

