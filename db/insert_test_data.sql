TRUNCATE fruit CASCADE;
SELECT setval('fruit_fruit_id_seq', 1, FALSE);

INSERT INTO fruit (name)
VALUES
	('Apple'),
	('Pear'),
	('Orange');



TRUNCATE ripeness CASCADE;
SELECT setval('ripeness_ripeness_id_seq', 1, FALSE);

-- Apple ripeness grades
INSERT INTO ripeness (
	name,
	fruit_id,
	minimum_storage_span
) VALUES
	('Grade A', 1, 0),
	('Grade B', 1, 20),
	('Grade C', 1, 50);

-- Pear ripeness grades
INSERT INTO ripeness (
	name,
	fruit_id,
	minimum_storage_span
) VALUES
	('Pear-1', 2, 0),
	('Pear-2', 2, 10),
	('Pear-3', 2, 25),
	('Pear-4', 2, 35);

-- Orange ripeness grades
INSERT INTO ripeness (
	name,
	fruit_id,
	minimum_storage_span
) VALUES
	('Good', 3, 0),
	('Bad', 3, 100);



TRUNCATE region CASCADE;
SELECT setval('region_region_id_seq', 1, FALSE);

INSERT INTO region (
	name,
	area,
	level
) VALUES
	('Eferding', 'Upper Austria', 271),
	('Graz', 'Styria', 353),
	('Barcelona', 'Spain', 9);



TRUNCATE batch CASCADE;
SELECT setval('batch_batch_id_seq', 1, FALSE);

INSERT INTO batch (
	fruit_id,
	year,
	month,
	amount,
	storage_date,
	region_id,
	ripeness_id
) VALUES (
	(SELECT fruit_id FROM fruit WHERE name = 'Apple'),
	2018, 06,
	1200,
	DATE('2018-06-12'),
	(SELECT region_id FROM region WHERE name = 'Eferding'),
	(SELECT ripeness_id FROM ripeness WHERE name = 'Grade A')
), (
	(SELECT fruit_id FROM fruit WHERE name = 'Pear'),
	2018, 07,
	700,
	DATE('2018-07-30'),
	(SELECT region_id FROM region WHERE name = 'Graz'),
	(SELECT ripeness_id FROM ripeness WHERE name = 'Pear-2')
), (
	(SELECT fruit_id FROM fruit WHERE name = 'Orange'),
	2018, 11,
	2500,
	DATE('2018-11-05'),
	(SELECT region_id FROM region WHERE name = 'Barcelona'),
	(SELECT ripeness_id FROM ripeness WHERE name = 'Good')
);

INSERT INTO batch (
	fruit_id,
	year,
	month
) VALUES (
	(SELECT fruit_id FROM fruit WHERE name = 'Pear'),
	2019, 05
);



TRUNCATE weather CASCADE;
SELECT setval('weather_weather_id_seq', 1, FALSE);

-- Weather for Eferding
INSERT INTO weather (
	region_id,
	year,
	month,
	rainy_days,
	sunny_days
) VALUES
	(1, 	2018, 06,	  4, 22),
	(1, 	2018, 07,	  1, 30),
	(1, 	2019, 04,	 12, 10);

INSERT INTO weather (
	region_id,
	year,
	month
) VALUES
	(1, 	2019, 05);

-- Weather for Graz
INSERT INTO weather (
	region_id,
	year,
	month,
	rainy_days,
	sunny_days
) VALUES
	(2,		2018, 06,	 0, 29),
	(2,		2018, 07,	 0, 31),
	(2,		2018, 12,	 2,  0),
	(2,		2019, 04,	16, 10);

-- Weather for Barcelona
INSERT INTO weather (
	region_id,
	year,
	month,
	rainy_days,
	sunny_days
) VALUES
	(3,		2018, 06,	 0, 30),
	(3,		2018, 07,	 0, 31),
	(3,		2018, 12,	10,  6),
	(3,		2019, 04,	 5, 18);
