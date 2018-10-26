import React from "react";
import ReactDom from "react-dom";

import "bootstrap/dist/css/bootstrap.min.css";

import AppRouter from "./router";

ReactDom.render(<AppRouter />, document.getElementById("container"));
