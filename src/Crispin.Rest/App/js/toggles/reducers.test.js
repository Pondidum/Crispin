import reducer from "./reducers";
import freeze from "deep-freeze";

import {
  FETCH_ALL_TOGGLES_STARTED,
  FETCH_ALL_TOGGLES_FINISHED,
  UPDATE_TOGGLE_NAME_STARTED,
  UPDATE_TOGGLE_NAME_FINISHED,
  UPDATE_TOGGLE_DESCRIPTION_STARTED,
  UPDATE_TOGGLE_DESCRIPTION_FINISHED,
  CREATE_TOGGLE_STARTED,
  CREATE_TOGGLE_FINISHED
} from "./actions";

describe("fetching all toggles", () => {
  it("should mark the collection as updating when starting", () => {
    const initialState = freeze({});
    const state = reducer(initialState, { type: FETCH_ALL_TOGGLES_STARTED });
    expect(state.updating).toBe(true);
  });

  it("should mark the collection as no updating when finishing", () => {
    const initialState = freeze({});
    const state = reducer(initialState, { type: FETCH_ALL_TOGGLES_FINISHED });
    expect(state.updating).toBe(false);
  });

  it("should replace the existing collection items with new", () => {
    const initialState = freeze({ all: ["one", "two"] });
    const event = freeze({
      type: FETCH_ALL_TOGGLES_FINISHED,
      toggles: ["three", "four", "five"]
    });
    const state = reducer(initialState, event);

    expect(state.all).toEqual(["three", "four", "five"]);
  });
});
