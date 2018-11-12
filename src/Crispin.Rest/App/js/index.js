import React from "react";
import ReactDom from "react-dom";
import { createStore, applyMiddleware, compose } from "redux";
import thunk from "redux-thunk";

import reducers from "./reducers";
import AppRouter from "./router";
import { fetchAllToggles } from "./toggles/actions";

import "bootstrap/dist/css/bootstrap.min.css";
import "../css/ui.css";

const composeEnhancers = window.__REDUX_DEVTOOLS_EXTENSION_COMPOSE__ || compose;

const store = createStore(reducers, composeEnhancers(applyMiddleware(thunk)));
store.dispatch(fetchAllToggles());

ReactDom.render(<AppRouter store={store} />, document.getElementById("root"));
