import React from "react";
import ReactDom from "react-dom";
import { createStore } from "redux";

import reducers from "./reducers";
import AppRouter from "./router";

import "bootstrap/dist/css/bootstrap.min.css";

const store = createStore(reducers);

const Root = <AppRouter store={store} />;

ReactDom.render(Root, document.getElementById("container"));
