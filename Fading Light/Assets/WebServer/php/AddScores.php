<?php
        // Configuration
        $hostname = '23.229.199.73';
        $username = 'bigbyte';
        $password = 'hungrygames';
        $database = 'i676540_bb2';
 
        $secretKey = "secretKey"; // Change this value to match the value stored in the client javascript below 
 
        try {
            $dbh = new PDO('mysql:host='. $hostname .';dbname='. $database, $username, $password);
        } catch(PDOException $e) {
            echo '<h1>An error has ocurred.</h1><pre>', $e->getMessage() ,'</pre>';
        }
 
        $realHash = md5($_GET['name'] . $_GET['gold'] . $secretKey); 
        
        if($realHash == $hash) { 
            $sth = $dbh->prepare('INSERT INTO highscores VALUES (:name, :datatime, :gold, :p1damagegiven, :p2damagegiven, :p1damagetaken, :p2damagetaken, :p1accuracy, :p2accuracy")');
            try {
                $sth->execute($_GET);
            } catch(Exception $e) {
                echo '<h1>An error has ocurred.</h1><pre>', $e->getMessage() ,'</pre>';
            }
        } 
?>
PHP