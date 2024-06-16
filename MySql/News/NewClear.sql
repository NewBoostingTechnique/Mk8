DROP procedure IF EXISTS `NewClear`;

CREATE PROCEDURE `NewClear` ()
BEGIN
  DELETE FROM news;
END;
