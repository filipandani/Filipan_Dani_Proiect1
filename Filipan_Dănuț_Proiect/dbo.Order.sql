CREATE TABLE [dbo].[Order] (
    [OrderID]    INT IDENTITY (1, 1) NOT NULL,
    [CustomerID] INT NOT NULL,
    [DrinkID]    INT NOT NULL,
    [OrderDate] DATETIME NULL, 
    CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED ([OrderID] ASC),
    CONSTRAINT [FK_Order_Customer_CustomerID] FOREIGN KEY ([CustomerID]) REFERENCES [dbo].[Customer] ([CustomerID]) ON DELETE CASCADE,
    CONSTRAINT [FK_Order_Drink_DrinkID] FOREIGN KEY ([DrinkID]) REFERENCES [dbo].[Drink] ([ID]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_Order_CustomerID]
    ON [dbo].[Order]([CustomerID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Order_DrinkID]
    ON [dbo].[Order]([DrinkID] ASC);

