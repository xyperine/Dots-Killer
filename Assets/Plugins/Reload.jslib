var MyPlugin = {

 reloadPage: function (){
        window.location.reload();
      }
};

mergeInto(LibraryManager.library, MyPlugin);