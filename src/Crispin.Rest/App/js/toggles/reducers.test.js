import reducer from "./reducers";

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
    const state = reducer({}, { type: FETCH_ALL_TOGGLES_STARTED });
    expect(state.updating).toBe(true);
  });
});
