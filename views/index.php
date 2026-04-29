<!DOCTYPE html>
<html lang="fr">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>AutoRent - Véhicules</title>
    <link rel="stylesheet" href="public/lobby.css">
</head>

<body>
<?php include("views/navbar.php"); ?>

<section class="section-hero">
    <div class="texte-hero">
        <h1>AutoRent — Partez où vous voulez,<br><span>quand vous voulez</span></h1>
        <p>Performance · Élégance · Confort</p>
        <a href="#vehicules" class="bouton-voir-vehicules">Voir les véhicules</a>
    </div>
</section>
<section class="section-import" style="padding: 20px; text-align: center; background: #f9f9f9; border-bottom: 1px solid #ddd;">
    <form action="" method="POST" enctype="multipart/form-data">
        <label style="font-weight: bold; margin-right: 10px;">Ajouter des véhicules via CSV :</label>
        <input type="file" name="file_csv" accept=".csv" required>
        <button type="submit" name="submit_import" class="bouton-filtrer">Ajouter à la liste</button>
    </form>
    <?php if(isset($_GET['success'])) echo "<p style='color:green;'>Fichier ajouté avec succès !</p>"; ?>
</section>

<section class="section-liste-voitures">
    <h2 id="vehicules" class="titre-section">Liste des véhicules</h2>
    <p class="sous-titre-section">Filtrez par type de véhicule pour trouver votre bonheur</p>

    <form class="formulaire-filtre" method="GET">
        <select class="liste-deroulante-type" name="type_voiture">
            <option value="">-- Tous les types --</option>
            <option value="Voiture" <?php if($typeSelectionne == "Voiture") echo "selected"; ?>>Voiture</option>
            <option value="Moto" <?php if($typeSelectionne == "Moto") echo "selected"; ?>>Moto</option>
            <option value="CampingCar" <?php if($typeSelectionne == "CampingCar") echo "selected"; ?>>Camping-Car</option>
        </select>
        <button class="bouton-filtrer" type="submit">Filtrer</button>
    </form>

    <div class="grille-voitures">
        <?php 
        foreach ($vehicules as $vehicule) { 
        ?>
            <div class="carte-voiture">
                <div class="entete-carte">
                    <?php if(($vehicule['Etat']) == 'Disponible'){ ?>
                        <span class="badge-disponible"><?php echo $vehicule['Etat']; ?></span>
                    <?php } elseif(($vehicule['Etat']) == 'Loué') {?>
                        <span class="badge-loue"><?php echo $vehicule['Etat']; ?></span>
                    <?php } else { ?>
                        <span class="badge-reparation"><?php echo $vehicule['Etat']; ?></span>
                    <?php } ?>
                    
                    <span class="plaque-immat"><?= $vehicule['Immatriculation'] ?></span>
                </div>

                <div class="nom-vehicule"><?= $vehicule['Marque'].' '.$vehicule['Modele'] ?></div>
                <div class="prix-bleu"><?= $vehicule['Prix'] ?> € <small>/ jour</small></div>
                
                <div class="ligne-separateur"></div>
                
                <p class="info-voiture"><strong>Kilométrage :</strong> <?= $vehicule['Km'] ?> km</p>

                <div class="details-techniques">
                    <?php if(($vehicule['Type']) == 'Voiture'): ?>
                        <p class="info-voiture"><strong>Places :</strong> <?= $vehicule['Divers1'] ?? 'N/A' ?></p>
                        <p class="info-voiture"><strong>Climatisation :</strong> <?= $vehicule['Divers2'] ?? 'N/A' ?></p>

                    <?php elseif(($vehicule['Type']) == 'Moto'): ?>
                        <p class="info-voiture"><strong>Cylindrée :</strong> <?= $vehicule['Divers1'] ?? 'N/A' ?> cc</p>
                        <p class="info-voiture"><strong>Permis :</strong> <?= $vehicule['Divers2'] ?? 'N/A' ?></p>

                    <?php else: // Camping-Car ?>
                        <p class="info-voiture"><strong>Longueur :</strong> <?= $vehicule['Divers1'] ?? 'N/A' ?> m</p>
                        <p class="info-voiture"><strong>Couchages :</strong> <?= $vehicule['Divers2'] ?? 'N/A' ?></p>
                    <?php endif; ?>
                </div>
            </div>
        <?php 
        } 
        ?>
    </div>
</section>

</body>
</html>