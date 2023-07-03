CREATE DATABASE BOOKSTORE
GO
USE BOOKSTORE
GO


DROP TABLE 

CREATE TABLE BOOK(
	ID INT,
	NAME NVARCHAR(100),
	DESCRIPTION NVARCHAR(MAX),
	AUTHOR NVARCHAR(1000),
	PRICE FLOAT,
	IMG VARCHAR(1000),
	COST FLOAT,
	STOCK INT,
	QUANTITY_SOLD INT,
	RATING FLOAT,
	CONSTRAINT PK_BOOK PRIMARY KEY (ID)
)

INSERT INTO BOOK VALUES(1,'As a Man Thinketh',NULL,'James Allen',59,'https://cdn.discordapp.com/attachments/1094565023847829564/1106820626590732359/jmHrbyaTb3c6aRym7Cbook1.png',20,20,0,NULL);
UPDATE BOOK SET DESCRIPTION = 'In As a Man Thinketh, James Allen points out the power of thoughts in shaping our realities. Often, we think that we are the victims of circumstance while in truth our thoughts, actions, and habits create the circumstances we dislike. The solution is to cultivate better thoughts just like we would treat a garden. Everyone should read it.
Can you think of a single moment in the whole day when your mind is blank and thoughtless?Do you know how powerful every thought is?“Cherish your visions; cherish your ideals; cherish the music that stirs in your heart, the beauty that forms in your mind, the loveliness that drapes your purest thoughts, for out of them will grow all delightful conditions, all heavenly environment; of these, if you but remain true to them, your world will at last be built.”Giving an insight into the power of thoughts; the effect they have on our health, body and circumstances; and how we become what we think; this compelling literary essay by James Allen contains practical wisdom which will inspire, enlighten and help us discover our hidden powers.Written in a spiritual tone, As a Man Thinketh has been a valuable source of inspiration ever since its first publication in 1903. It continues to remain a classic bestseller.'
INSERT INTO BOOK VALUES(2,'The Jungle Book','In The Jungle Book, a young boy named Mowgli becomes a member of the Seeonee Wolf Pack. A cruel tiger named Shere Khan plots against Mowgli and the leader of his pack, Akela. When Mowgli grows up, he realizes that he must rejoin the ranks of men.Mowgli strays from his village one day. After being attacked by Shere Khan, he''s saved by Father Wolf, who asks Akela, the leader of the wolves, to accept Mowgli as a member of the pack.Mowgli briefly returns to the world of men, but leaves after he learns that Shere Khan has been plotting against Akela. He defeats the tiger, but knows that someday he will rejoin the man-pack.A python named Kaa takes Mowgli down to the Cold Lairs, where he steals an ankus. He discards the ankus, fearing its deadly curse. This results in the death of six men. After this incident, Mowgli becomes unhappy and gradually drifts toward the world of men.','Rudyard Kipling',99,'https://cdn.discordapp.com/attachments/1094565023847829564/1106821965278355526/cvLwYNXyje5f45bW7C2.png',20,20,0,NULL)
INSERT INTO BOOK VALUES(3,'The Story of My Life','The Story of My Life Summary: The Story of my life is the story of Helen Keller who triumphed over adversity and became world famous. She was born on June 27, 1880 in Tuscumbia, Alabama. Her parents were Captain Arthur Henry Keller, a confederate army veteran and a newspaper editor and Kate Adams Keller. She was born as a normal child. But at the age of 19 months, she suffered an illness that left her deaf and blind. Her family wondered how a deaf and blind child could be educated. At the age of six, her mother managed to get a teacher, Anne Sullivan, to teach Helen. After studying at the Wright Humason School for the Deaf and the Cambridge School for Young ladies, she entered Radcliff College in 1900 and finished her graduation in 1904.The Story of My Life is a true example that Helen’s life is neither a miracle nor a joke. With the help of her teacher, Anne Sullivan, Helen became an inter-nationally recognized and respected figure. In 1908 Helen published “The World I Live In”, an account of how she experienced the world through touch, taste and scent.','Helen Keller',149,'https://cdn.discordapp.com/attachments/1094565023847829564/1106822780202270760/3tHrxL1JReWwXWUv7C3.png',20,20,0,NULL)
INSERT INTO BOOK VALUES(4,'Othello','Othello takes place in 16th-century Venice and also Cyprus. Othello who is a noble black warrior in the Venetian army that secretly married a beautiful white woman called Desdemona who is the daughter of a prominent senator named Brabantio. When he eventually finds out and is completely furious he decides to disown Desdemona.Iago has a secret jealousy and resentment towards Othello because a soldier named Lieutenant Cassio has been put in front of him and also suspects that Othello has been cheating with his wife. Waiting on revenge, Iago plans a devious comeback to plant suspicions in Othello’s mind that Desdemona has been having an affair with Cassio. He decided to start a street fight which Cassio is blamed for, and is then dismissed from his post by Othello. Desdemona takes up Cassio’s case with her husband, which only increases his suspicions that the pair are lovers.While all of this is happening Iago manages to find a treasured handkerchief from Desdemona that was given to her by Othello. He somehow gets the handkerchief on Cassio so that Othello sees it and he finally concludes that the possession is proof of the affair. Due to the jealousy, he orders Iago to murder Cassio. Then Othello decides to strangle Desdemona. Immediately afterwards her innocence is revealed, and Iago’s treachery exposed. In a fit of grief and remorse Othello kills himself and Iago is taken into custody by the authorities.','William Shakespeare',70,'https://cdn.discordapp.com/attachments/1094565023847829564/1108312309123797082/QShoIhh4WPwnKb827C71cH7dI2kIS.png',20,20,0,NULL)

select * from book;





CREATE TABLE [USER](
	ID INT ,
	USERNAME VARCHAR(50) UNIQUE,
	PASSWORD VARCHAR(1000),
	FULLNAME NVARCHAR(100),
	EMAIL VARCHAR(1000),
	PHONE VARCHAR(12),
	BIRTHDATE DATE,
	CREATED_TIME DATETIME,
	TOKEN VARCHAR(1000),
	SALT VARCHAR(5),
	CONSTRAINT PK_USER PRIMARY KEY(ID)
)

CREATE UNIQUE INDEX USERNAME_INDEX ON [USER](USERNAME)

select * from [user]
select * from ADDRESS_USER

CREATE TABLE ADDRESS_USER(
	ID_USER INT,
	SERIAL INT,
	ID_PROVINCE INT,
	ID_DISTRICT INT,
	ID_WARD VARCHAR(7),
	HOUSE_NUMBER NVARCHAR(1000),
	IS_DEFAULT INT,
	CONSTRAINT PK_ADDRESS_USER PRIMARY KEY(ID_USER,SERIAL)
)


ALTER TABLE ADDRESS_USER
ADD CONSTRAINT FK_ADDRESS_USER_2_USER
FOREIGN KEY (ID_USER) REFERENCES [USER](ID);


CREATE TABLE PROVINCE(
	ID_PROVINCE INT,
	NAME NVARCHAR(1000),
	CONSTRAINT PK_PROVINCE PRIMARY KEY(ID_PROVINCE)
)

CREATE TABLE DISTRICT(
	ID_DISTRICT INT,
	ID_PROVINCE INT,
	NAME NVARCHAR(1000),
	CONSTRAINT PK_DISTRICT PRIMARY KEY(ID_DISTRICT)
)


CREATE TABLE WARD(
	ID_WARD VARCHAR(7),
	ID_DISTRICT INT,
	NAME NVARCHAR(1000),
	CONSTRAINT PK_WARP PRIMARY KEY(ID_WARD,ID_DISTRICT)
)

ALTER TABLE ADDRESS_USER
ADD CONSTRAINT FK_ADDRESS_USER_2_PROVINCE
FOREIGN KEY (ID_PROVINCE) REFERENCES PROVINCE(ID_PROVINCE);

ALTER TABLE ADDRESS_USER
ADD CONSTRAINT FK_ADDRESS_USER_2_DISTRICT
FOREIGN KEY (ID_DISTRICT) REFERENCES DISTRICT(ID_DISTRICT);

ALTER TABLE ADDRESS_USER
ADD CONSTRAINT FK_ADDRESS_USER_2_WARD
FOREIGN KEY (ID_WARD,ID_DISTRICT) REFERENCES WARD(ID_WARD,ID_DISTRICT);




--SELECT * FROM PROVINCE
--SELECT * FROM DISTRICT
--SELECT * FROM WARD


ALTER TABLE DISTRICT
ADD CONSTRAINT FK_DISTRICT_2_PROVINCE
FOREIGN KEY (ID_PROVINCE) REFERENCES PROVINCE(ID_PROVINCE);

ALTER TABLE WARD
ADD CONSTRAINT FK_WARD_2_DISTRICT
FOREIGN KEY (ID_DISTRICT) REFERENCES DISTRICT(ID_DISTRICT);


CREATE TABLE CART(
	ID INT,
	TOTAL FLOAT,
	AMOUNT INT,
	ID_USER INT,
	CONSTRAINT PK_CART PRIMARY KEY(ID)
)
ALTER TABLE CART
ADD CONSTRAINT FK_CART_USER
FOREIGN KEY (ID_USER) REFERENCES [USER](ID);


CREATE TABLE CART_ITEM(
	ID_CART INT,
	ID_BOOK INT,
	AMOUNT INT,
	TOTAL FLOAT,
	CONSTRAINT PK_CART_ITEM PRIMARY KEY(ID_CART,ID_BOOK)
)

ALTER TABLE CART_ITEM
ADD CONSTRAINT FK_CART_ITEM_2_CART
FOREIGN KEY (ID_CART) REFERENCES CART(ID);

ALTER TABLE CART_ITEM
ADD CONSTRAINT FK_CART_ITEM_BOOK
FOREIGN KEY (ID_BOOK) REFERENCES BOOK(ID);


CREATE TABLE SHIP_METHOD(
	ID INT,
	NAME NVARCHAR(1000),
	PRICE FLOAT,
	CONSTRAINT PK_SHIP_METHOD PRIMARY KEY(ID)
)


INSERT INTO SHIP_METHOD VALUES(1,'Express delivery 24 hour ',12)
INSERT INTO SHIP_METHOD VALUES(3,'Ship in 2-3 days',2)

CREATE TABLE [ORDER](
	ID_ORDER INT,
	ID_USER INT,
	SERIAL INT,
	TOTAL FLOAT,
	FEESHIP FLOAT,
	TYPESHIP VARCHAR(100),
	SHIPINFO VARCHAR(1000),
	NOTE VARCHAR(1000),
	STATUS VARCHAR(100), --waiting payment,shipping,shipped,cancel
	REASON VARCHAR(100),
	ID_PAYMENT VARCHAR(100),
	TYPE_PAYMENT VARCHAR(100),
	CONSTRAINT PK_ORDER PRIMARY KEY(ID_ORDER)
)



SELECT * FROM [ORDER]
select * from PAYMENT_PAYPAL
SELECT * FROM ORDER_ITEM

CREATE TABLE ORDER_ITEM(
	ID_ORDER INT,
	ID_BOOK INT,
	AMOUNT INT,
	TOTAL FLOAT,
	ID_EVENT INT,
	RATE INT,
	CONSTRAINT PK_ORDER_ITEM PRIMARY KEY(ID_ORDER,ID_BOOK)
)

CREATE TABLE PAYMENT_PAYPAL(
	ID_PAYMENT VARCHAR(100),
	STATUS VARCHAR(100), --Pending / Paid / cancel
	CREATETIME DATETIME,
	EXPIRES_IN INT,
	LINKCHECKOUT VARCHAR(1000),
	LINKCHECK VARCHAR(1000),
	ID_PAYER VARCHAR(100),
	
	CONSTRAINT PK_PAYMENT PRIMARY KEY(ID_PAYMENT),
)




CREATE TABLE PAYMENT_METHOD(
	ID INT,
	NAME NVARCHAR(1000)
)

INSERT INTO PAYMENT_METHOD VALUES(1,'Paypal')
INSERT INTO PAYMENT_METHOD VALUES(2,'Zalo Pay')


ALTER TABLE [ORDER]
ADD CONSTRAINT FK_ORDER_2_USER
FOREIGN KEY (ID_USER) REFERENCES [USER](ID);

ALTER TABLE [ORDER]
ADD CONSTRAINT FK_ORDER_2_ADDRESS
FOREIGN KEY (ID_USER,SERIAL) REFERENCES ADDRESS_USER(ID_USER,SERIAL);

ALTER TABLE [ORDER]
ADD CONSTRAINT FK_ORDER_2_PAYMENT_PAYPAL
FOREIGN KEY (ID_PAYMENT) REFERENCES PAYMENT_PAYPAL(ID_PAYMENT);

ALTER TABLE ORDER_ITEM
ADD CONSTRAINT FK_ORDER_ITEM_2_ORDER
FOREIGN KEY (ID_ORDER) REFERENCES [ORDER](ID_ORDER);

ALTER TABLE ORDER_ITEM
ADD CONSTRAINT FK_ORDER_ITEM_2_BOOK
FOREIGN KEY (ID_BOOK) REFERENCES BOOK(ID);

select * from BOOK

