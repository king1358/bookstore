CREATE DATABASE BOOKSTORE
GO
USE BOOKSTORE
GO

CREATE TABLE BOOK(
	ID VARCHAR(10),
	NAME NVARCHAR(100),
	DESCRIPTION NVARCHAR(MAX),
	PRICE FLOAT,
	AUTHOR NVARCHAR(100),
	SOURCEIMG VARCHAR(1000)
	CONSTRAINT PK_BOOK PRIMARY KEY (ID)
)



INSERT INTO BOOK VALUES('B1','As a Man Thinketh',NULL,59,'James Allen','https://cdn.discordapp.com/attachments/1094565023847829564/1106820626590732359/jmHrbyaTb3c6aRym7Cbook1.png')
UPDATE BOOK SET DESCRIPTION = 'In As a Man Thinketh, James Allen points out the power of thoughts in shaping our realities. Often, we think that we are the victims of circumstance while in truth our thoughts, actions, and habits create the circumstances we dislike. The solution is to cultivate better thoughts just like we would treat a garden. Everyone should read it.
Can you think of a single moment in the whole day when your mind is blank and thoughtless?Do you know how powerful every thought is?“Cherish your visions; cherish your ideals; cherish the music that stirs in your heart, the beauty that forms in your mind, the loveliness that drapes your purest thoughts, for out of them will grow all delightful conditions, all heavenly environment; of these, if you but remain true to them, your world will at last be built.”Giving an insight into the power of thoughts; the effect they have on our health, body and circumstances; and how we become what we think; this compelling literary essay by James Allen contains practical wisdom which will inspire, enlighten and help us discover our hidden powers.Written in a spiritual tone, As a Man Thinketh has been a valuable source of inspiration ever since its first publication in 1903. It continues to remain a classic bestseller.'
INSERT INTO BOOK VALUES('B10','The Jungle Book','In The Jungle Book, a young boy named Mowgli becomes a member of the Seeonee Wolf Pack. A cruel tiger named Shere Khan plots against Mowgli and the leader of his pack, Akela. When Mowgli grows up, he realizes that he must rejoin the ranks of men.Mowgli strays from his village one day. After being attacked by Shere Khan, he''s saved by Father Wolf, who asks Akela, the leader of the wolves, to accept Mowgli as a member of the pack.Mowgli briefly returns to the world of men, but leaves after he learns that Shere Khan has been plotting against Akela. He defeats the tiger, but knows that someday he will rejoin the man-pack.A python named Kaa takes Mowgli down to the Cold Lairs, where he steals an ankus. He discards the ankus, fearing its deadly curse. This results in the death of six men. After this incident, Mowgli becomes unhappy and gradually drifts toward the world of men.',99,'Rudyard Kipling','https://cdn.discordapp.com/attachments/1094565023847829564/1106821965278355526/cvLwYNXyje5f45bW7C2.png')
INSERT INTO BOOK VALUES('B2','The Story of My Life','The Story of My Life Summary: The Story of my life is the story of Helen Keller who triumphed over adversity and became world famous. She was born on June 27, 1880 in Tuscumbia, Alabama. Her parents were Captain Arthur Henry Keller, a confederate army veteran and a newspaper editor and Kate Adams Keller. She was born as a normal child. But at the age of 19 months, she suffered an illness that left her deaf and blind. Her family wondered how a deaf and blind child could be educated. At the age of six, her mother managed to get a teacher, Anne Sullivan, to teach Helen. After studying at the Wright Humason School for the Deaf and the Cambridge School for Young ladies, she entered Radcliff College in 1900 and finished her graduation in 1904.The Story of My Life is a true example that Helen’s life is neither a miracle nor a joke. With the help of her teacher, Anne Sullivan, Helen became an inter-nationally recognized and respected figure. In 1908 Helen published “The World I Live In”, an account of how she experienced the world through touch, taste and scent.',149,'Helen Keller','https://cdn.discordapp.com/attachments/1094565023847829564/1106822780202270760/3tHrxL1JReWwXWUv7C3.png')
INSERT INTO BOOK VALUES('B3','Othello','Othello takes place in 16th-century Venice and also Cyprus. Othello who is a noble black warrior in the Venetian army that secretly married a beautiful white woman called Desdemona who is the daughter of a prominent senator named Brabantio. When he eventually finds out and is completely furious he decides to disown Desdemona.Iago has a secret jealousy and resentment towards Othello because a soldier named Lieutenant Cassio has been put in front of him and also suspects that Othello has been cheating with his wife. Waiting on revenge, Iago plans a devious comeback to plant suspicions in Othello’s mind that Desdemona has been having an affair with Cassio. He decided to start a street fight which Cassio is blamed for, and is then dismissed from his post by Othello. Desdemona takes up Cassio’s case with her husband, which only increases his suspicions that the pair are lovers.While all of this is happening Iago manages to find a treasured handkerchief from Desdemona that was given to her by Othello. He somehow gets the handkerchief on Cassio so that Othello sees it and he finally concludes that the possession is proof of the affair. Due to the jealousy, he orders Iago to murder Cassio. Then Othello decides to strangle Desdemona. Immediately afterwards her innocence is revealed, and Iago’s treachery exposed. In a fit of grief and remorse Othello kills himself and Iago is taken into custody by the authorities.',70,' William Shakespeare','https://cdn.discordapp.com/attachments/1094565023847829564/1108312309123797082/QShoIhh4WPwnKb827C71cH7dI2kIS.png')

SELECT * FROM BOOK


CREATE TABLE [USER](
	USERNAME VARCHAR(50),
	PASSWORD VARCHAR(50),
	FULLNAME NVARCHAR(100),
	TOKEN VARCHAR(1000),
	ID VARCHAR(10) UNIQUE,
	SALT VARCHAR(5),
	CONSTRAINT PK_USER PRIMARY KEY(USERNAME)
)

ALTER TABLE [USER]
ALTER COLUMN PASSWORD VARCHAR(1000);

INSERT INTO [USER] VALUES('tototete','111',N'Nguyễn Tài',NULL,'U1','ADMIN');
SELECT * FROM [USER];
SELECT COUNT(*) FROM [USER]



UPDATE [USER] SET PASSWORD = 'B8819835748D1E4D2EE77D948D07CB0C3EBBE47F' WHERE USERNAME = 'tototete'

CREATE TABLE CART(
	ID VARCHAR(10),
	TOTAL FLOAT,
	ID_U VARCHAR(10),
	STATUS VARCHAR(100), --wait,delivery,received
	TIME_CHECKOUT DATETIME,
	CONSTRAINT PK_CART PRIMARY KEY(ID)
)




ALTER TABLE CART
ADD CONSTRAINT FK_CART_USER
FOREIGN KEY (ID_U) REFERENCES [USER](ID);


CREATE TABLE CART_ITEM(
	ID_C VARCHAR(10),
	ID_B VARCHAR(10),
	AMOUNT INT,
	TOTAL FLOAT,
	CONSTRAINT PK_CART_ITEM PRIMARY KEY(ID_C,ID_B)
)

ALTER TABLE CART_ITEM
ADD CONSTRAINT FK_CART_ITEM_CART
FOREIGN KEY (ID_C) REFERENCES CART(ID);

ALTER TABLE CART_ITEM
ADD CONSTRAINT FK_CART_ITEM_BOOK
FOREIGN KEY (ID_B) REFERENCES BOOK(ID);

SELECT COUNT(*) FROM CART WHERE ID_U = 'U1'

SELECT * FROM CART;
SELECT * FROM CART_ITEM


DELETE CART_ITE

UPDATE CART_ITEM SET AMOUNT = 2, TOTAL = 99*2 WHERE ID_C = 'C_U1' AND ID_B = 'B10'

delete CART_ITEM;
delete CART;

select * from cart_item;

UPDATE CART SET TOTAL = TOTAL + 15 WHERE ID_U = 'U1'