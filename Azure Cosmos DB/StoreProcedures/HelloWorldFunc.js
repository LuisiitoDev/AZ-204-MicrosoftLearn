
var helloWorldStoreProc = {
    id: "helloWorld",
    serverScript: function(){

        // Permite el acceso a todas las acciones que se pueden realizar dentro de CosmosDB y
        // acceso a los objetos de solicitud y respuesta.
        var context =  getContext(); 
    
        var response = context.getResponse();

        response.setBody("Hello, World!");
    }
};


var createDocumentStoreProc = {
    id: "createMyDocument",
    body: function createMyDocument(documentToCreate){
        var context = getContext();
        var collection = context.getCollection();
        var accepted = collection.createDocumento(collection.getSelfLink(),
            documentToCreate,
            function(err, documentToCreate){
                if(err) throw new Error("Error " + err.message);
                context.getResponse().setBody(documentToCreate.id);
            });
        if(!accepted) return;
    }
};