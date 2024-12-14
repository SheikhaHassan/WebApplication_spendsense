﻿CREATE TABLE [dbo].[User]
(
	[user_id] INT NOT NULL PRIMARY KEY,
	[name] VARCHAR(50) NOT NULL,
	[username] VARCHAR(50), 
	[email] VARCHAR(50) UNIQUE NOT NULL,
	[password] VARCHAR(255) NOT NULL,
	[role] VARCHAR(10) DEFAULT 'user',
	
);

INSERT INTO [dbo].[User] ([user_id], [name], [username], [email], [password], [role]) 
VALUES 
(1, 'Maysoon', 'May11', 'maysoon11@gmail.com', '123456', 'user'),
(2, 'Sheikha', 'sh123', 'sh123@gmail.com', '987654', 'admin'),
(3, 'Zainab ', 'Zainab9', 'Zainab9@gmail.com', '24680', 'user'),
(4, 'Suhaila', 'Suhaila118', 'Suhaila118@gmail.com', '13579', 'user');



CREATE TABLE [dbo].[Categories]
(
	[category_id] INT PRIMARY KEY IDENTITY(1,1), 
	[category_name] VARCHAR(50) NOT NULL,      
	[parent_category_id] INT NULL,             
	[created_at] DATETIME DEFAULT GETDATE(),  
	CONSTRAINT [fK_ParentCategory] FOREIGN KEY ([parent_category_id]) REFERENCES [dbo].[Categories] ([category_id])
);


INSERT INTO [dbo].[Categories] ([category_name], [parent_category_id]) 
VALUES 
('Utilities', NULL),
('Groceries', NULL),
('Entertainment', NULL),
('Clothing', NULL),
('Water', 1),                        
('Electricity', 1),
('Internet', 1);



CREATE TABLE [dbo].[Expense]
(
	[expense_id] INT PRIMARY KEY IDENTITY(1,1),  
    	[user_id] INT NOT NULL,                      
    	[category_id] INT NOT NULL,                  
    	[amount] DECIMAL(10, 2) NOT NULL,           
    	[expense_date] DATE NOT NULL,                
    	[created_at] DATETIME DEFAULT GETDATE(),   
    	CONSTRAINT [fK_UserExpense] FOREIGN KEY ([user_id]) REFERENCES [dbo].[User] ([user_id]),
    	CONSTRAINT [fK_CategoryExpense] FOREIGN KEY ([category_id]) REFERENCES [dbo].[Categories] ([category_id]),
	
);

INSERT INTO [dbo].[Expense] ([user_id], [category_id], [amount], [expense_date]) 
VALUES 
(1, 2, 120.00, '2024-11-30'),   -- Groceries for User 1
(2, 6, 75.00, '2024-11-29'),    -- Electricity for User 2
(3, 7, 50.00, '2024-11-28'),    -- Internet for User 3
(4, 5, 40.00, '2024-11-27'),    -- Water for User 4
(1, 3, 20.00, '2024-11-26'),    -- Entertainment for User 1
(2, 4, 150.00, '2024-11-25'),   -- Clothing for User 2
(3, 5, 35.00, '2024-11-24'),    -- Water for User 3
(4, 2, 80.00, '2024-11-23'),    -- Groceries for User 4
(1, 6, 100.00, '2024-11-22');   -- Electricity for User 1



CREATE TABLE [dbo].[Budget]
(
	[budget_id] INT PRIMARY KEY IDENTITY(1,1),   
    	[user_id] INT NOT NULL,                      
   	[total_income] DECIMAL(10, 2) NOT NULL,     
    	[needs] DECIMAL(10, 2) NOT NULL,            
    	[wants] DECIMAL(10, 2) NOT NULL,             
    	[savings] DECIMAL(10, 2) NOT NULL,           
    	[created_at] DATETIME DEFAULT GETDATE(),   
	CONSTRAINT [fK_UserBudget] FOREIGN KEY ([user_id]) REFERENCES [dbo].[User] ([user_id])
);


INSERT INTO [dbo].[Budget] ([user_id], [total_income], [needs], [wants], [savings]) 
VALUES 
(1, 3000.00, 1500.00, 1000.00, 500.00),
(2, 4000.00, 1600.00, 1200.00, 1200.00),
(3, 2500.00, 1200.00, 800.00, 500.00),
(4, 2000.00, 1000.00, 700.00, 300.00);



CREATE TABLE [dbo].[UserCategorySpending] (
    [record_id]     INT          IDENTITY (1, 1) NOT NULL,
    [user_id]       INT          NOT NULL,
    [category_name] VARCHAR (50) NOT NULL,
    [amount_spent]  DECIMAL (18) DEFAULT ((0)) NULL,
    PRIMARY KEY CLUSTERED ([record_id] ASC),
    CONSTRAINT [fk_UserSpending] FOREIGN KEY ([user_id]) REFERENCES [dbo].[User] ([user_id]) ); 



CREATE VIEW [dbo].[DashboardSummary] AS
SELECT 
    b.user_id AS UserId,
    b.total_income AS TotalIncome,
    ISNULL(SUM(e.amount), 0) AS TotalExpense,
    b.total_income - ISNULL(SUM(e.amount), 0) AS RemainingBudget
FROM 
    [dbo].[Budget] b
LEFT JOIN 
    [dbo].[Expense] e ON b.user_id = e.user_id
GROUP BY 
    b.user_id, b.total_income; 

-- SELECT 
  --  TotalIncome, 
   -- TotalExpense, 
   -- RemainingBudget
-- FROM 
   -- [dbo].[DashboardSummary]
-- WHERE 
   -- UserId = @UserId;