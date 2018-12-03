import reducer from "./reducers";
import freeze from "deep-freeze";

import {
  FETCH_SYSTEM_INFO_STARTED,
  FETCH_SYSTEM_INFO_FINISHED
} from "./actions";

describe("when fetching system information", () => {
  const initialState = {};

  it("should mark the info object as updating when starting", () => {
    const event = { type: FETCH_SYSTEM_INFO_STARTED };
    const state = reducer(initialState, event);

    expect(state.info).toEqual({ updating: true });
  });

  it("should fill the info when completed", () => {
    const event = {
      type: FETCH_SYSTEM_INFO_FINISHED,
      payload: { conditionTypes: ["one", "two", "three"] }
    };
    const state = reducer(initialState, event);

    expect(state.info).toEqual({
      updating: false,
      conditionTypes: ["one", "two", "three"]
    });
  });
});
