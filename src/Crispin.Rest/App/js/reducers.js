import { combineReducers } from "redux";
import system from "./system/reducers";
import toggles from "./toggles/reducers";

export default combineReducers({
  toggles,
  system
});
