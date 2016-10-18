<?php
        $hostname = 'localhost';
        $username = 'bigbyte';
        $password = 'hungrygames';
        $database = 'i676540_bb2';
 
        $secretKey = "2W_0Yc:p_~oU}(P1?]P98)1]0894J0";

        try {
            $dbh = new PDO('mysql:host='. $hostname .';dbname='. $database, $username, $password);
        } catch(PDOException $e) {
            echo '<h1>An error has occurred.</h1><pre>', $e->getMessage() ,'</pre>';
        }
         
        $sth = $dbh->query('SELECT * FROM highscores ORDER BY minutes ASC, seconds ASC');
        $sth->setFetchMode(PDO::FETCH_ASSOC);
     
        $result = $sth->fetchAll();
        
        
        if(count($result) > 0) {
            foreach($result as $r) {
                echo json_encode($r);
                echo "\n";
            }
        }

?>
