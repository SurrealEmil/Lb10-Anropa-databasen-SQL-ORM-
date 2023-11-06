--H�mta alla produkter med deras namn, pris och kategori namn. Sortera p� kategori namn och sen produkt namn.
SELECT Products.ProductName, Products.UnitPrice, Categories.CategoryName
FROM Products
JOIN Categories
ON Products.CategoryID = Categories.CategoryID
ORDER BY Categories.CategoryName, Products.ProductName;


--H�mta alla kunder och antal ordrar de gjort. Sortera fallande p� antal ordrar.
SELECT Customers.ContactName, COUNT(Orders.OrderID) AS AmountOfOrders
FROM Customers
JOIN Orders
ON Customers.CustomerID = Orders.CustomerID
GROUP BY Customers.ContactName
ORDER BY AmountOfOrders DESC;


--H�mta alla anst�llda tillsammans med territorie de har hand om (EmployeeTerritories och Territories tabellerna)
SELECT CONCAT(Employees.FirstName, ' ', Employees.LastName) AS FullName, Territories.TerritoryDescription
FROM Employees
JOIN EmployeeTerritories
ON Employees.EmployeeID = EmployeeTerritories.EmployeeID
JOIN Territories
ON EmployeeTerritories.TerritoryID = Territories.TerritoryID