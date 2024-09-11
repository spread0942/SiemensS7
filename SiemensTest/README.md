# Test Siemens

Progetto con l'unico scopo di testare le librerie Siemens e la riuscita di interazione.

Per comunicare con il protocollo Siemens dobbiamo avere la libreria S7netplus. In questo progetto è già attiva, ma per installare seguire i seguenti passaggi: `Project > Manage NuGet Packages..` e cerchiamo il pacchetto S7netplus. Di questo pacchetto troviamo diverse informazioni in [GitHub](https://github.com/S7NetPlus/s7netplus/wiki).

Intanto per connettersi dobbiamo far parte del mondo Siemens, per farlo dobbiamo conntterci via **Ethernet**. Una volta collegati alla macchina individuaiamo i vari indirizzi IP, ce ne sono di diverso tipo, potrebbe esserci il router (in questo caso è 192.168.0.100), potrebbe esserci il PLC (in questo caso 192.168.0.101) ed invece dovrebbe esserci quello che interessa a noi e che ci farà leggere la parte di memoria (in questo caso 192.168.0.102).
Come nel file MainWindows, andremo ad instanziare la nostra classe PLC, dove gli assegnamo il tipo di CPU (in questo caso è la S71200, consultabile da pdf), l'indirizzo IP (in questo caso 192.168.0.102) e per la CPU 1200 il rack è a 0 e stazione di posizione 1. Una volta creato il nostro oggetto, andiamo ad aprire la connessione con la macchina, se va a buon fine allora possiamo ad andare o leggere o scrivere le variabili ([Lista dei possibili errori](https://github.com/S7NetPlus/s7netplus/wiki#error-handling).

Per la lettura dobbiamo utilizzare il metodo `S7.Read()`, abbiamo due versioni. Per quella che accetta una sola stringa di input dobbiamo seguire questa logica //<nome database>.<nome variabile>. Il nome database lo troviamo nel file pdf e nel nostro caso è DB100. Per le variabili la logica è la seguente, iniziare con DB, il terzo dato specificherà il tipo di dato da leggere (ad esempio se sarà W leggerà un Word, se sarà D leggerà Dint, se sarà X leggerà un solo bit all'interno di un 8bit, quindi un dato boleano e così via) nel nostro caso sarà X per i boleani e D per gli Int32. Ed infine specificheremo il nostro bite ed i suoi bit da leggere.
Per esempio:

* DB100.DBX0.1 >> significhe che nel database DB100, nella variabile boleanna nel spazio di memoria del bite 0, leggeremo il bit 1. Che nel nostro caso consiste nel "Stato Start Ciclo Continuo"
* DB100.DBD22.0 >> significa che nel database DB100, nella variabile Dint (ovvero Int a 32 bit) leggeremo il bite 22. Per noi equivale al "Stato Macchina"

Ricordarsi appunto che le variabili laborano con 8bit, con le variabili di tipo word potrebbero nascere dei problemi, perché per logiche vengono cambiati dei bit, ma non dovrebbe essere un problema nostro per il momento perché non abbiamo word.