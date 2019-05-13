DROP TABLE fruit CASCADE;
DROP TABLE ripeness CASCADE;
DROP TABLE batch CASCADE;

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

CREATE TABLE batch (
	batch_id INTEGER PRIMARY KEY,
	fruit_id INTEGER,
	year INTEGER,
	month INTEGER,
	amount INTEGER,
	storage_date DATE,
	ripeness_id INTEGER,

	FOREIGN KEY (fruit_id) REFERENCES fruit(fruit_id),
	FOREIGN KEY (ripeness_id) REFERENCES ripeness(ripeness_id),
	UNIQUE(fruit_id, year, month)
);
