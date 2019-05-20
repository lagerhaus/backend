DROP TABLE fruit CASCADE;
DROP TABLE ripeness CASCADE;
DROP TABLE region CASCADE;
DROP TABLE batch CASCADE;
DROP TABLE weather CASCADE;


CREATE TABLE fruit (
	fruit_id INTEGER PRIMARY KEY,
	name VARCHAR UNIQUE
);

CREATE TABLE ripeness (
	ripeness_id INTEGER PRIMARY KEY,
	name VARCHAR,
	fruit_id INTEGER,
	minimum_storage_span INTEGER,

	FOREIGN KEY (fruit_id) REFERENCES fruit(fruit_id),
	UNIQUE(name, fruit_id)
);

CREATE TABLE region (
	region_id INTEGER PRIMARY KEY,
	name VARCHAR UNIQUE,
	area VARCHAR,
	level INTEGER
);

CREATE TABLE batch (
	batch_id INTEGER PRIMARY KEY,
	fruit_id INTEGER,
	year INTEGER,
	month INTEGER,
	amount INTEGER,
	storage_date DATE,
	region_id INTEGER,
	ripeness_id INTEGER,

	FOREIGN KEY (fruit_id) REFERENCES fruit(fruit_id),
	FOREIGN KEY (region_id) REFERENCES region(region_id),
	FOREIGN KEY (ripeness_id) REFERENCES ripeness(ripeness_id),
	UNIQUE(fruit_id, year, month)
);

CREATE TABLE weather (
	weather_id INTEGER PRIMARY KEY,
	region_id INTEGER,
	year INTEGER,
	month INTEGER,
	rainy_days INTEGER,
	sunny_days INTEGER,
	
	FOREIGN KEY (region_id) REFERENCES region(region_id),
	UNIQUE(region_id, year, month)
);
