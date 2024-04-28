DROP procedure IF EXISTS `NewsList`;

DELIMITER $$
CREATE PROCEDURE `NewsList` ()
BEGIN
  SELECT
    persons.Name AS 'AuthorPersonName',
    news.Body AS 'Body',
    news.Date as 'Date',
    news.Title AS 'Title'
  FROM
    News
    JOIN
      persons ON News.AuthorPersonId = persons.Id;
END$$

DELIMITER ;