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
      payload: ["three", "four", "five"]
    });
    const state = reducer(initialState, event);

    expect(state.all).toEqual(["three", "four", "five"]);
  });
});

describe("create toggle", () => {
  it("should mark the collection as updating when starting", () => {
    const initialState = freeze({ updating: false, all: [] });
    const state = reducer(initialState, { type: CREATE_TOGGLE_STARTED });
    expect(state.updating).toBe(true);
  });

  it("should mark the collection as updating when finishing", () => {
    const initialState = freeze({ updating: true, all: [] });
    const state = reducer(initialState, { type: CREATE_TOGGLE_FINISHED });
    expect(state.updating).toBe(false);
  });

  it("should append the new toggle to the existing toggles", () => {
    const initialState = freeze({
      updating: true,
      all: [{ name: "one" }, { name: "two" }]
    });
    const state = reducer(initialState, {
      type: CREATE_TOGGLE_FINISHED,
      payload: { name: "new" }
    });

    expect(state.all).toEqual([
      { name: "one" },
      { name: "two" },
      { name: "new" }
    ]);
  });
});

describe("updating a toggle name", () => {
  let initialState;
  beforeEach(
    () =>
      (initialState = freeze({
        all: [
          { id: 1, name: "one" },
          { id: 2, name: "two" },
          { id: 3, name: "three" }
        ]
      }))
  );

  it("should update the toggle name and set updating on", () => {
    const state = reducer(initialState, {
      type: UPDATE_TOGGLE_NAME_STARTED,
      payload: { toggleID: 2, name: "updated" }
    });

    expect(state.all).toEqual([
      { id: 1, name: "one" },
      { id: 2, name: "updated", updating: true },
      { id: 3, name: "three" }
    ]);
  });

  it("should update the toggle name and set updating off", () => {
    const state = reducer(initialState, {
      type: UPDATE_TOGGLE_NAME_FINISHED,
      payload: { toggleID: 2, name: "updated" }
    });

    expect(state.all).toEqual([
      { id: 1, name: "one" },
      { id: 2, name: "updated", updating: false },
      { id: 3, name: "three" }
    ]);
  });
});

describe("updating a toggle description", () => {
  let initialState;
  beforeEach(
    () =>
      (initialState = freeze({
        all: [
          { id: 1, name: "one", description: "aaa" },
          { id: 2, name: "two", description: "bbb" },
          { id: 3, name: "three", description: "ccc" }
        ]
      }))
  );

  it("should update the toggle name and set updating on", () => {
    const state = reducer(initialState, {
      type: UPDATE_TOGGLE_DESCRIPTION_STARTED,
      payload: { toggleID: 2, description: "updated" }
    });

    expect(state.all).toEqual([
      { id: 1, name: "one", description: "aaa" },
      { id: 2, name: "two", description: "updated", updating: true },
      { id: 3, name: "three", description: "ccc" }
    ]);
  });

  it("should update the toggle name and set updating off", () => {
    const state = reducer(initialState, {
      type: UPDATE_TOGGLE_DESCRIPTION_FINISHED,
      payload: { toggleID: 2, description: "updated" }
    });

    expect(state.all).toEqual([
      { id: 1, name: "one", description: "aaa" },
      { id: 2, name: "two", description: "updated", updating: false },
      { id: 3, name: "three", description: "ccc" }
    ]);
  });
});
