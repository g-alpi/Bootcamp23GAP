var modal = window.modal || {};
(function () {
  this.buttonOnClick = function () {
    var pageInput = {
      pageType: "webresource",
      webresourceName: "gap_AddProductModal.html",
    };
    var navigationOptions = {
      target: 2,
      height: { value: 30, unit: "%" },
      width: { value: 30, unit: "%" },
      position: 1,
    };
    Xrm.Navigation.navigateTo(pageInput, navigationOptions).then(
      function success() {
        // Run code on success
        console.log("Loaded");
      },
      function error() {
        // Handle errors
      }
    );
  };

  this.callCustomAction = function (addComicConfirmationValue) {
    let globalContext = parent.Xrm.Utility.getGlobalContext();
    let serverUrl = globalContext.getClientUrl();
    let userRoles = globalContext.userSettings.roles["_collection"];
    let isAdmin = false;
    isAdmin = Object.values(userRoles).some(
      (role) => role["name"] === "System Administrator"
    );
    let actionName = "gap_MyCustomAction";

    let InputParamValue = document.getElementById("comicId").value;

    let data = {
      MyInputParam: InputParamValue,
      AddComicConfirmation: addComicConfirmationValue,
      IsAdmin: isAdmin,
    };
    let request = new XMLHttpRequest();
    request.open("POST", `${serverUrl}/api/data/v9.2/${actionName}`, true);
    request.setRequestHeader("Accept", "application/json");
    request.setRequestHeader("Content-Type", "application/json; charset=utf-8");
    request.setRequestHeader("OData-MaxVersion", "4.0");
    request.setRequestHeader("OData-Version", "4.0");

    request.onreadystatechange = function () {
      if (this.readyState === 4) {
        request.onreadystatechange = null;

        if (this.status === 200 || this.status === 204) {
          console.log("Action Called Successfully...");
          result = JSON.parse(this.response);
          console.log(result);
          if (document.querySelectorAll(".divModal").length >= 1) {
            document.querySelectorAll(".divModal")[0].remove();
          }

          if (result["ComicExist"]) {
            comicExistDOM(result["CoverURL"], result["MyOutputParam"]);
          } else {
            addComicDOM();
          }
        } else {
          var error = JSON.parse(this.response).error;
          console.log("Error in Action: " + error.message);
        }
      }
    };
    request.send(window.JSON.stringify(data));

    function addComicDOM() {
      let button = document.querySelector("button");
      let divElement = document.createElement("div");
      let paragraphElement = document.createElement("p");
      let textNode = document.createTextNode(
        "El comic no existe. ¿Quieres añadirlo?"
      );
      let buttonSiElement = document.createElement("button");
      let buttonNoElement = document.createElement("button");

      // Agregar contenido y atributos a los elementos
      paragraphElement.appendChild(textNode);
      buttonSiElement.appendChild(document.createTextNode("Si"));
      buttonNoElement.appendChild(document.createTextNode("No"));

      // Agregar elementos al div principal
      divElement.classList.add("divModal");
      divElement.appendChild(paragraphElement);
      divElement.appendChild(buttonSiElement);
      divElement.appendChild(buttonNoElement);
      button.after(divElement);
      buttonSiElement.addEventListener("click", callCustomActionWithUpdateData);
    }

    function comicExistDOM(imgUrl, comicTitle) {
      let button = document.getElementById("callCustomActionButton");
      let divElement = document.createElement("div");

      let imgElement = document.createElement("img");
      let h1Element = document.createElement("h3");

      imgElement.setAttribute("src", imgUrl);
      imgElement.setAttribute("alt", "thumbnail");
      h1Element.textContent = comicTitle;

      divElement.classList.add("coverContainerModal");
      divElement.classList.add("divModal");
      divElement.appendChild(h1Element);
      divElement.appendChild(imgElement);
      button.after(divElement);
    }

    function callCustomActionWithUpdateData() {
      data["AddComicConfirmation"] = true;
      console.log(data);
      request.open("POST", `${serverUrl}/api/data/v9.2/${actionName}`, true);
      request.setRequestHeader("Accept", "application/json");
      request.setRequestHeader(
        "Content-Type",
        "application/json; charset=utf-8"
      );
      request.setRequestHeader("OData-MaxVersion", "4.0");
      request.setRequestHeader("OData-Version", "4.0");
      request.onreadystatechange = function () {
        if (this.readyState === 4) {
          request.onreadystatechange = null;

          if (this.status === 200 || this.status === 204) {
            console.log("Action Called Successfully...");
          }
        } else {
          var error = JSON.parse(this.response).error;
          console.log("Error in Action: " + error.message);
        }
      };
      request.send(window.JSON.stringify(data));
    }
  };
}).call(modal);
