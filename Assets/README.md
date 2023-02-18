#README

## PROJECT ORGANIZATION
Il progetto è organizzato in modo da separare i vari tipi di asset.

> Assets <br>
+--- Animations <br>
| +--- MainMenu <br>
| +--- ... <br>
+--- Audio <br>
| +--- Music <br>
| +--- Sounds <br>
| +--- Speech <br>
+--- Development <br>
| +--- Michele <br>
| +--- Simone <br>
| +--- Stefano <br>
+--- Scripts <br>
| +--- MainMenu <br>
| +--- StartUp <br>
| +--- ... <br>

In ogni cartella si può procedere dividendo per argomento, per le cose di carattere generale si possono lasciare nella cartella principale.

### Development
Qui mettere le cose in fase di sviluppo, ognuno nelle proprie cartelle.

> Development <br>
+--- Michele <br>
| +--- coso1 <br>
| +--- coso2 <br>
| +--- ... <br>
+--- Simone <br>
| +--- MainMenu <br>
| +--- StartUp <br>
| +--- ... <br>
+--- Stefano <br>
| +--- ... <br>

Quando si raggiunge uno stadio di sviluppo (semi)definitivo, spostare nelle relative cartelle definitive in root. In questo modo dovremmo evitare problemi in fase di merging.

### Struttura delle scene
Tutte le scene si trovano nella cartella "Scenes". Video di start up e menù principale sono in due scene e si accede ad esse attraverso indicizzazione.

Inoltre, ogni ambiente ha la sua scena. Ci sono già due scene da popolare, "Environment_X" con X indice intero in modo da poter accedere alla scena sia per nome che per indice. Nel dettaglio:

0) StartUp
1) MainMenu
2) Environment_1   //per ambiente demo 1
3) Environment_2   //per ambiente demo 2
4) Environment_N   //per ambiente caricato da utente

