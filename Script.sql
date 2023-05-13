CREATE DATABASE BOOKSTORE
GO
USE BOOKSTORE
GO

CREATE TABLE BOOK(
	ID VARCHAR(8),
	NAME NVARCHAR(100),
	DESCRIPTION NVARCHAR(MAX),
	PRICE FLOAT,
	AUTHOR NVARCHAR(100),
	SOURCEIMG VARCHAR(1000)
	CONSTRAINT PK_BOOK PRIMARY KEY (ID)
)


INSERT INTO BOOK VALUES('B1','As a Man Thinketh',NULL,59,'James Allen','https://cdn.discordapp.com/attachments/1094565023847829564/1106820626590732359/jmHrbyaTb3c6aRym7Cbook1.png')
UPDATE BOOK SET DESCRIPTION = 'In As a Man Thinketh, James Allen points out the power of thoughts in shaping our realities. Often, we think that we are the victims of circumstance while in truth our thoughts, actions, and habits create the circumstances we dislike. The solution is to cultivate better thoughts just like we would treat a garden. Everyone should read it.
Can you think of a single moment in the whole day when your mind is blank and thoughtless?Do you know how powerful every thought is?�Cherish your visions; cherish your ideals; cherish the music that stirs in your heart, the beauty that forms in your mind, the loveliness that drapes your purest thoughts, for out of them will grow all delightful conditions, all heavenly environment; of these, if you but remain true to them, your world will at last be built.�Giving an insight into the power of thoughts; the effect they have on our health, body and circumstances; and how we become what we think; this compelling literary essay by James Allen contains practical wisdom which will inspire, enlighten and help us discover our hidden powers.Written in a spiritual tone, As a Man Thinketh has been a valuable source of inspiration ever since its first publication in 1903. It continues to remain a classic bestseller.'
INSERT INTO BOOK VALUES('B10','The Jungle Book','In The Jungle Book, a young boy named Mowgli becomes a member of the Seeonee Wolf Pack. A cruel tiger named Shere Khan plots against Mowgli and the leader of his pack, Akela. When Mowgli grows up, he realizes that he must rejoin the ranks of men.Mowgli strays from his village one day. After being attacked by Shere Khan, he''s saved by Father Wolf, who asks Akela, the leader of the wolves, to accept Mowgli as a member of the pack.Mowgli briefly returns to the world of men, but leaves after he learns that Shere Khan has been plotting against Akela. He defeats the tiger, but knows that someday he will rejoin the man-pack.A python named Kaa takes Mowgli down to the Cold Lairs, where he steals an ankus. He discards the ankus, fearing its deadly curse. This results in the death of six men. After this incident, Mowgli becomes unhappy and gradually drifts toward the world of men.',99,'Rudyard Kipling','https://cdn.discordapp.com/attachments/1094565023847829564/1106821965278355526/cvLwYNXyje5f45bW7C2.png')
INSERT INTO BOOK VALUES('B2','The Story of My Life','The Story of My Life Summary: The Story of my life is the story of Helen Keller who triumphed over adversity and became world famous. She was born on June 27, 1880 in Tuscumbia, Alabama. Her parents were Captain Arthur Henry Keller, a confederate army veteran and a newspaper editor and Kate Adams Keller. She was born as a normal child. But at the age of 19 months, she suffered an illness that left her deaf and blind. Her family wondered how a deaf and blind child could be educated. At the age of six, her mother managed to get a teacher, Anne Sullivan, to teach Helen. After studying at the Wright Humason School for the Deaf and the Cambridge School for Young ladies, she entered Radcliff College in 1900 and finished her graduation in 1904.The Story of My Life is a true example that Helen�s life is neither a miracle nor a joke. With the help of her teacher, Anne Sullivan, Helen became an inter-nationally recognized and respected figure. In 1908 Helen published �The World I Live In�, an account of how she experienced the world through touch, taste and scent.',149,'Helen Keller','https://cdn.discordapp.com/attachments/1094565023847829564/1106822780202270760/3tHrxL1JReWwXWUv7C3.png')

SELECT * FROM BOOK