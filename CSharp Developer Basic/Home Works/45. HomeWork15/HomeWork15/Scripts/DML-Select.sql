select c."ID" CustomerID
       , c."FirstName"
       , c."LastName"
       , p."ID" ProductID
       , o."Quantity" ProductQuantity
       , p."Price" ProductPrice
from public."Orders" o
join public."Customers" c on o."CustomerID" = c."ID"
join public."Products" p on o."ProductID" = p."ID"
where c."Age" > 30 and p."ID" = 1;