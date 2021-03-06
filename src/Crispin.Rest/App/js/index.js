import React from "react";
import ReactDom from "react-dom";
import { createStore, applyMiddleware, compose } from "redux";
import thunk from "redux-thunk";
import { apiMiddleware } from "redux-api-middleware";

import reducers from "./reducers";
import AppRouter from "./router";

import { fetchAllToggles } from "./toggles/actions";
import { fetchSystemInfo } from "./system/actions";

import "bootstrap/dist/css/bootstrap.min.css";
import "../css/ui.css";

const composeEnhancers = window.__REDUX_DEVTOOLS_EXTENSION_COMPOSE__ || compose;

const store = createStore(
  reducers,
  composeEnhancers(applyMiddleware(thunk, apiMiddleware))
);

store.dispatch(fetchAllToggles());
store.dispatch(fetchSystemInfo());

ReactDom.render(<AppRouter store={store} />, document.getElementById("root"));
