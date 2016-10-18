<?php
        $hostname = 'localhost';
        $username = 'bigbyte';
        $password = 'hungrygames';
        $database = 'i676540_bb2';
 
        $secretKey = "2W_0Yc:p_~oU}(P1?]P98)1]0894J0";

        try {
            $dbh = new PDO('mysql:host='. $hostname .';dbname='. $database, $username, $password);
            $dbh->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
            $realHash = md5($_GET['name'] . $_GET['gold'] . $secretKey); 
            
            //Check to make sure the passed hash is the same as the generated one
            if($realHash == $_GET['hash']) { 
                $sth = $dbh->prepare('INSERT INTO highscores  (name, gold, p1accuracy, p2accuracy, minutes, seconds, monsterskilled, timeskilled, chestsmissed)  
                VALUES (:name, :gold, :p1accuracy, :p2accuracy, :minutes, :seconds, :monsterskilled, :timeskilled, :chestsmissed)');

                try {
                    //Remove the hash from the get arary
                    array_splice($_GET, 9, 1);
                    $sth->execute($_GET);
                } catch(Exception $e) {
                    echo '<h1>An error has ocurred.</h1><pre>', $e->getMessage() ,'</pre>';
                }
            } 
            
        } catch(PDOException $e) {
            echo '<h1>An error has ocurred.</h1><pre>', $e->getMessage() ,'</pre>';
        }

?>
