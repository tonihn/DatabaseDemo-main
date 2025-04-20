INSERT INTO carriers (
    carrier_name,
    contact_url,
    contact_phone
)
VALUES
    ('DHL', 'https://www.dhl.com', '+49 228 767 676'),
    ('UPS', 'https://www.ups.com', '+1 800 742 5877');

SELECT * FROM carriers;

UPDATE orders
SET "CarrierId"      = 5,        -- DHL
    tracking_number = 'DH123456789',
    shipped_date    = NOW(),
    order_status    = 'Shipped'
WHERE order_id = 1;

 SELECT o.order_id,
        o.order_status,
        c.carrier_name,
        o.tracking_number,
        o.shipped_date,
        o.delivered_date
 FROM orders o
 LEFT JOIN carriers c ON o."CarrierId" = c."CarrierId"
 WHERE o.order_id = 1;


UPDATE orders
SET delivered_date = NOW(),
    order_status   = 'Delivered'
WHERE order_id = 1;

SELECT o.order_id,
       o.order_status,
       c.carrier_name,
       o.tracking_number,
       o.shipped_date,
       o.delivered_date
FROM orders o
LEFT JOIN carriers c ON o."CarrierId" = c."CarrierId"
WHERE o.order_id = 1;