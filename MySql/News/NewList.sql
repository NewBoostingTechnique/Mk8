DROP procedure IF EXISTS `NewList`;

CREATE PROCEDURE `NewList` ()
BEGIN
  SELECT
    persons.Name AS 'AuthorPersonName',
    news.Body AS 'Body',
    news.Date as 'Date',
    news.Title AS 'Title'
  FROM
    News
    JOIN
      persons ON News.AuthorPersonId = persons.Id
  ORDER BY
    news.Date DESC;
END;